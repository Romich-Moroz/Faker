using System;
using System.Collections.Generic;

namespace FakerLib.PluginSupport
{
    public interface IGenerator
    {
        
    }
    public abstract class Generator<T> : IGenerator
    {
        public abstract T Generate();

        public List<T> GenerateList()
        {
            Random r = new Random();
            List<T> tmp = new List<T>();
            for (int i = 0; i < r.Next(50); i++)
            {
                tmp.Add(Generate());
            }
            return tmp;
        }
    }
}
