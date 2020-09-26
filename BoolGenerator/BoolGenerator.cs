using FakerLib.PluginSupport;
using System;

namespace BoolGenerator
{
    public class BoolGenerator : GeneratorPlugin<bool>
    {
        public override string PluginName => "Bool generator for FakerLib";

        public override bool Generate()
        {
            return Convert.ToBoolean(new Random().Next() & 0b1);
        }
    }
}
