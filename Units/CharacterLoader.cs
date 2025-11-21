using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Units
{
    //to be used later when we want to load characters from json.
    public static class CharacterLoader
    {
        public static Character LoadFromFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return null;

        }
    }
}
