using System;
using FakerLib.PluginSupport;

namespace FakerLib.Generators
{
    class DoubleGenerator : Generator<double>
    {
        public override double Generate()
        {
            return new Random().NextDouble();
        }
    }
}
