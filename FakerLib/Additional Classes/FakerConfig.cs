using FakerLib.PluginSupport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace FakerLib
{
    public class FakerConfig
    {
        private List<Rule> rules;

        public void AddRule<T, K, G>(Expression<K> exp) where G : Generator<K>
        {
            rules.Add(new Rule(typeof(T), typeof(K), typeof(G), exp));
        }
    }
}
