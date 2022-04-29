-------------------------------------------------------------------------------
--Sim City (SNES) lua script for SNES9X. Logs data (palettes and city layout) in RAM to files.

local LogsFolderPal = "F:\\spel\\snes\\emus\\snes9x-rr-1.60-win32-x64\\Saves\\simcitylogs\\palettes\\"
local LogsFolderMap = "F:\\spel\\snes\\emus\\snes9x-rr-1.60-win32-x64\\Saves\\simcitylogs\\maps\\"

local Addresses = {
	month = 0x7E0B55,
	palettes = 0x7E2440,
	mapCursor = 0x7E0B2D, --Map select cursor location. 0=NEXT button, 1=OK button, 2-7=The 6 number inc/dec buttons.
	mapNumSel = 0x7E0B27, --Selected map number (3 bytes, one per digit).
	mapNumLoad = 0x7E0B2A, --Currently loaded map number (3 bytes, one per digit)
	mapTiles = 0x7F0200, --Map tiles (120*100*2 bytes)
}
--0x7E0B2A MSB affects map generator? 0=Normal set, 1=Alt set?
--Reset to 0xFF at start or after map is started (why going back via save/load 'goto menu' activates the alt set).

local GloPal = {
	monthLast = 0,
	isLoggingPalettes = false,
	palettes = {{}, {}, {}, {}, {}, {}, {}, {}},
	palettesLast = {},
}

local MapSelState = {
	isChangingNext = 1,
	isChangingPress = 2,
	isChangingOk = 3,
	isLoading = 4,
	isSaving = 5,
	isDone = 6,
}

local GloMap = {
	selState = nil,
	saveMapNum = 0,
	doAltMapSet = false,
	tileData = {},
}

-------------------------------------------------------------------------------
--snes9x-rr-1.60-win32-x64 doesn't have 'emu.frameadvance()' so we have to do input some other way.
--Joypad input table = {R, L, X, A, right, left, down, up, start, select, Y, B}
local GloInp = {
	buttonsToPress = nil,
	frameCountPress = 0,
	frameCountRelease = 0,
}

function setInputDef(inputTable)
	setInput(inputTable, 13, 3)
end

function setInput(inputTable, framesToPress, framesToRelease) --Call this to set input for joypad 1.
	local frameCount = emu.framecount()
	GloInp.buttonsToPress = inputTable
	GloInp.frameCountPress = frameCount + framesToPress
	GloInp.frameCountRelease = frameCount + framesToPress + framesToRelease
end

function processInput() --Call to process any input. Returns true if input was processed.
	if GloInp.buttonsToPress ~= nil then	
		local frameCount = emu.framecount()
		if frameCount < GloInp.frameCountPress then --Press buttons.
			joypad.set(1, GloInp.buttonsToPress)
			return true
		elseif frameCount < GloInp.frameCountRelease then --Release buttons.
			joypad.set(1, {})
			return true
		end
		
		GloInp.buttonsToPress = nil
	end
	return false
end

-------------------------------------------------------------------------------

function memReadU8(addr)
	return memory.readbyte(addr)
end

function memWriteU8(addr, value)
	memory.writebyte(addr, value)
end

function memReadU16(addr)
	return memReadU16LoHi(addr, addr + 1)
end

function memReadU16LoHi(addrLo, addrHi)
	return bit.bor(memReadU8(addrLo), bit.lshift(memReadU8(addrHi), 8))
end

function memWriteU16(addr, value)
	memWriteU16LoHi(addr, addr + 1, value)
end

function memWriteU16LoHi(addrLo, addrHi, value)
	memWriteU8(addrLo, getLoByte(value))
	memWriteU8(addrHi, getHiByte(value))
end

function getLoByte(ushort)
	return bit.band(ushort, 0xFF)
end

function getHiByte(ushort)
	return bit.band(bit.rshift(ushort, 8), 0xFF)
end

-------------------------------------------------------------------------------

function logYearFrames()
	--Log how many frames there are per year.
	memory.registerwrite(Addresses.month, onMonthWriteYear) --Register callback for writes to month counter.
end

function onMonthWriteYear()
	local month = memReadU8(Addresses.month)
	
	if month == 5 and GloPal.monthLast == 0 then
		print("Frame count start = " .. emu.framecount())
		GloPal.monthLast = 5
	end
	
	if month == 5 and GloPal.monthLast == 4 then
		print("Frame count end = " .. emu.framecount())
		memory.registerwrite(Addresses.month, nil) --Unregister callback for writes to month counter.
	end
	
	if GloPal.monthLast ~= 0 then
		GloPal.monthLast = month
	end
end

-------------------------------------------------------------------------------

function logPalettes()
	--Log how palettes changes over the year's seasons.
	memory.registerwrite(Addresses.month, onMonthWritePalette) --Register callback for writes to month counter.
end

function onMonthWritePalette()
	local month = memReadU8(Addresses.month)
	print("month write = " .. month)
	if month == 1 and GloPal.monthLast == 13 then	--December to January new year? 13 is written, but quickly changed to 1.
		if not GloPal.isLoggingPalettes then
			print("Started logging palette changes!")			
			print("Frame count = " .. emu.framecount())
			
			--Store initial palettes.
			for i = 1, 8, 1 do
				local pal = getPalette(i)
				GloPal.palettes[i][1] = pal
				GloPal.palettesLast[i] = pal
			end
			
			GloPal.isLoggingPalettes = true
			emu.registerafter(checkPalettes) --Register callback for after a frame is done.
		else
			print("Ended logging palette changes!")
			print("Frame count = " .. emu.framecount())
			
			 --Save palette logs to files.
			for i = 1, 8, 1 do
				savePaletteLog(i, "palette" .. tostring(i) .. ".txt")
			end
			
			GloPal.isLoggingPalettes = false
			emu.registerafter(nil) --Unregister callback for after a frame is done.
			memory.registerwrite(Addresses.month, nil) --Unregister callback for writes to month counter.
		end
	end
	GloPal.monthLast = month
end

function checkPalettes()
	for i = 1, 8, 1 do
		checkPaletteChanges(i)
	end
end

function checkPaletteChanges(paletteIndex)
	local palLen = table.maxn(GloPal.palettes[paletteIndex])
	
	--Palette 7 is repeatedly cycling through the same values so only need a few samples.
	if paletteIndex == 7 and palLen > 40 then return end
	
	local pal = getPalette(paletteIndex)
	local palLast = GloPal.palettesLast[paletteIndex]
	for i = 2, 17, 1 do --Check if palette has changed since last frame.
		if pal[i] ~= palLast[i] then --If changed, store it.
			GloPal.palettes[paletteIndex][palLen + 1] = pal
			break
		end
	end
	GloPal.palettesLast[paletteIndex] = pal --Keep track of last frame's palette.
end

function getPalette(paletteIndex)
	local pal = {}
	local palAddr = Addresses.palettes + (32 * (paletteIndex - 1)) --32 bytes per palette (2 * 16).
	pal[0] = emu.framecount() --Store frame count at index 0 to see how many frames between changes.
	pal[1] = memReadU8(Addresses.month) --Store month at index 1.
	for i = 2, 17, 1 do
		pal[i] = memReadU16(palAddr)
		palAddr = palAddr + 2
	end
	return pal
end

function savePaletteLog(paletteIndex, fileName)
	local file,err = io.open(LogsFolderPal .. fileName, "w+")
	if err then print(err) end
	
	local pal = GloPal.palettes[paletteIndex]
	print("Palette " .. tostring(paletteIndex) .. " changes count = " .. table.maxn(pal))
	for i = 1, table.maxn(pal), 1 do
		local palRow = pal[i]
		file:write(string.format("%8d", palRow[0]) .. ", ") --Frame count.
		file:write(string.format("%2d", palRow[1]) .. ", ") --Month count.
		for j = 2, 17, 1 do
			file:write(string.format("%4x", palRow[j]) .. ", ") --Palette entry 1-16.
		end
		file:write("\n")
	end
	
	file:close()
end

-------------------------------------------------------------------------------

function logMaps(doAltSet)
	print("log maps")
	emu.registerbefore(saveMaps) --Register callback for before a frame is done.
	GloMap.selState = MapSelState.isChangingNext
	GloMap.doAltMapSet = doAltSet
	--emu.speedmode("nothrottle") --Not available in snes9x-rr-1.60-win32-x64?
end

function saveMaps()
	if not processInput() then --No input to process?
		if GloMap.selState == MapSelState.isChangingNext then
			moveToNextButton()
		elseif GloMap.selState == MapSelState.isChangingPress then
			pressNextButton()
		elseif GloMap.selState == MapSelState.isChangingOk then
			moveToOkButton()
		elseif GloMap.selState == MapSelState.isLoading then
			waitUntilMapLoaded()
		elseif GloMap.selState == MapSelState.isSaving then
			saveMapTileData()
		elseif GloMap.selState == MapSelState.isDone then
			logMapsEnd()
		end
	end
end

function moveToNextButton()
	local cursorLoc = memReadU8(Addresses.mapCursor)
	if cursorLoc ~= 0 then --Cursor isn't on NEXT button? Then move it up.
		setInputDef({up=true})
	else
		--print("Cursor is on NEXT button!")
		GloMap.selState = MapSelState.isChangingPress
	end
end

function pressNextButton()
	local selNum = getSelectedMapNum()
	if selNum ~= GloMap.saveMapNum then --Press NEXT button until desired map number is selected.
		setInputDef({B=true})
	else
		--print("Pressed NEXT button!")
		GloMap.selState = MapSelState.isChangingOk
		
		if GloMap.doAltMapSet then --Need to adjust for alternative map set?
			memWriteU8(Addresses.mapNumLoad, 0xFF) --Activate alternative map set.
		else
			memWriteU8(Addresses.mapNumLoad, 0x00) --Do normal map set.
		end
	end
end

function moveToOkButton()
	local cursorLoc = memReadU8(Addresses.mapCursor)
	if cursorLoc ~= 1 then --Cursor isn't on OK button? Then move it down.
		setInputDef({down=true})
	else
		--print("Cursor is on OK button!")
		GloMap.selState = MapSelState.isLoading
	end
end

function waitUntilMapLoaded()
	--Wait until selected map is loaded.
	local loadNum = getLoadedMapNum()
	if GloMap.saveMapNum == loadNum then
		--print("Map is loaded, saving it!")
		GloMap.selState = MapSelState.isSaving
	end
end

function saveMapTileData()
	local mapNum = GloMap.saveMapNum
	
	local dataInd = mapNum * 24000 --120*100*2=24000 bytes per map.
	for i = 0, 24000-1, 1 do
		GloMap.tileData[dataInd + i] = memReadU8(Addresses.mapTiles + i)
	end
	
	--print("Map is saved, change it!")
	
	--if mapNum >= 10 then --Debug testing!
	if mapNum >= 999 then
		GloMap.selState = MapSelState.isDone
	else
		GloMap.saveMapNum = mapNum + 1
		GloMap.selState = MapSelState.isChangingNext
	end
end

function logMapsEnd()
	print("log maps end")
	
	local fileName = "MapsTileData"
	if GloMap.doAltMapSet then
		fileName = fileName .. "Alt.bin"
	else
		fileName = fileName .. ".bin"
	end
	
	local file,err = io.open(LogsFolderMap .. fileName, "w+b")
	if err then print(err) end
	
	--file:write(string.char(unpack(GloMap.tileData))) --Doesn't work?
	
	local byteCount = table.maxn(GloMap.tileData)
	for i = 0, byteCount, 1 do
		file:write(string.char(GloMap.tileData[i])) --string.char needed to get the binary value.
	end
	
	file:close()
	
	emu.registerbefore(nil) --Unregister callback for before a frame is done.
	--emu.speedmode("normal") --Not available in snes9x-rr-1.60-win32-x64?
end

function getSelectedMapNum() --Returns the currently selected map number.
	local selN1 = bit.band(memReadU8(Addresses.mapNumSel + 0), 0xF)
	local selN2 = bit.band(memReadU8(Addresses.mapNumSel + 1), 0xF)
	local selN3 = bit.band(memReadU8(Addresses.mapNumSel + 2), 0xF)
	local selNum = (selN1 * 1) + (selN2 * 10) + (selN3 * 100)
	return selNum
end

function getLoadedMapNum() --Returns the currently loaded map number.
	local loadN1 = bit.band(memReadU8(Addresses.mapNumLoad + 0), 0xF)
	local loadN2 = bit.band(memReadU8(Addresses.mapNumLoad + 1), 0xF)
	local loadN3 = bit.band(memReadU8(Addresses.mapNumLoad + 2), 0xF)
	local loadNum = (loadN1 * 1) + (loadN2 * 10) + (loadN3 * 100)
	return loadNum
end

-------------------------------------------------------------------------------

function main()
	--Frames per year seems to be around 6400?
	--To get 112 (4*7*4) animation frames you need to sample palettes every 57th frame (6400/112).
	--112 frames may be too much though (about 22 seconds)? Each season transition lasts for about
	--200 frames though so you need to sample pretty often to catch the effect.
	
	--logYearFrames()
	--logPalettes()
	logMaps(false) --Normal map set. Start script at map selector screen with map number 999 selected and loaded.
	--logMaps(true) --Alternative map set.
end

main()