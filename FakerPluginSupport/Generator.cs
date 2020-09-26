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
            List<T> tmp = new List<T>();
            for (int i = 0; i < new Random().Next(10); i++)
            {
                tmp.Add(Generate());
            }
            return tmp;
        }
    }
}
