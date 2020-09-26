using FakerLib.PluginSupport;
using System;
using System.Text;

namespace FakerLib.Generators
{
    class StringGenerator : Generator<string>
    {
        public override string Generate()
        {
            Random r = new Random();
            byte[] tmp = new byte[r.Next(15) * 2];
            r.NextBytes(tmp);
            return Encoding.UTF8.GetString(tmp);
        }
    }
}
