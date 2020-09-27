using FakerLib.PluginSupport;
using System;

namespace Lab2_Faker
{
    
    class CustomStringGenerator : Generator<string>
    {
        private string[] values = { "Arsonist", "Dungeon master", "Long latex gloves", "three hundred bucks", "do you like what you see", "that's amazing", "Take it boy", "as we can", "performance artist" };
        public override string Generate()
        {
            return values[new Random().Next(values.Length)];
        }
    }
}
