using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCityMapper
{
    //Sort of a wrapper around a state file so it can be treated as a save file with one city "saved" in it.
    class SaveFileState : SaveFile
    {
        private readonly City[] mCities;

        private SaveFileState(City[] cities)
        {
            mCities = cities;
        }

        public static SaveFileState open(StateFile state)
        {
            //Only one city "slot" in a state file.
            City[] cities = new City[] { SimCity.getCity(state.Ram) };
            return new SaveFileState(cities);

            //The 32KB SRAM is included in state files so it would also be possible to add any cities in
            //it to the array of cities. But I think it's a bit too surprising to the user to get up to
            //three cities/images from a state file. The user probably only expected/wanted the city he/she
            //saved a state of (city in WRAM).

            //This feature should be pretty easy to add if wanted though. Just provide the state file's SRAM
            //content to the SRAM file class and add the cities from it to the city array.
        }

        public override City[] getCities()
        {
            return mCities;
        }
    }
}
