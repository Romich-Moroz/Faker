using FakerLib.PluginSupport;
using System;

namespace FakerLib.Generators
{
    class DateTimeGenerator : Generator<DateTime>
    {
        public override DateTime Generate()
        {
            Random r = new Random();
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(r.Next(range));
        }
    }
}
