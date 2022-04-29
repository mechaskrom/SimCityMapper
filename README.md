# SimCityMapper
A command line tool that can save images of SimCity (SNES) cities.

## About
I wrote it to research/test a few things in the SNES game SimCity and to create
my contributions to [vgmaps](https://vgmaps.com/Atlas/SuperNES/index.htm#SimCity) and [gamefaqs](https://gamefaqs.gamespot.com/snes/588657-simcity/faqs).

## Usage
```
SimCityMapper infile [options]

Infile is path to a supported file type:
-Snes9x emulator savestate.
-ZSNES emulator savestate.
-32KB battery backup SRAM.

Options:
-s winter|spring|summer|autumn|fall   Override season in infile.
-a   Save as an animated image.
-f   Add a footer with some data about the city.

Example: SimCityMapper "C:\my cities\donuts.srm" -s autumn -a -f
```

## Example Output
![example animated](https://github.com/mechaskrom/SimCityMapper/blob/main/files/states/compare/example.state%20image.gif?raw=true)
