using FakerLib.Configuration;
using FakerLib.PluginSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2_Faker
{
    
    class CustomStringGenerator : Generator<string>
    {
        private string[] values = { "Arsonist", "Dungeon master", "Long latex glove", "three hundred bucks" };
        public override string Generate()
        {
            return values[new Random().Next(values.Length)];
        }
    }
}
