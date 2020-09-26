using System;
using FakerLib.PluginSupport;

namespace FakerLib.Generators
{
    class FloatGenerator : Generator<float>
    {
        public override float Generate()
        {
            return (float)new Random().NextDouble();
        }
    }
}
