using System;
using FakerLib.PluginSupport;

namespace CharGenerator
{
    public class CharGenerator : GeneratorPlugin<char>
    {

        public override string PluginName => "Char generator for FakerLib";

        public override char Generate()
        {
            return (char)new Random().Next(255);
        }
    }
}
