using System;
using FakerLib.PluginSupport;

namespace FakerLib.Generators
{
    class IntGenerator : Generator<int>
    {
        public override int Generate()
        {
            return new Random().Next();
        }
    }
}
