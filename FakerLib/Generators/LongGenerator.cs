using FakerLib.PluginSupport;
using System;

namespace FakerLib.Generators
{
    class LongGenerator : Generator<long>
    {
        public override long Generate()
        {
            Random r = new Random();
            return r.Next() << 32 | r.Next(); 
        }
    }
}
