using FakerLib.PluginSupport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace FakerLib.Configuration
{
    public class FakerConfig
    {
        private List<Rule> rules;

        public void AddRule<T, K, G>(Expression<Action<T>> exp) where G : Generator<K>
        {
            rules.Add(new Rule(typeof(T), typeof(K), typeof(G), exp));
        }

        public List<Rule> GetCreationRules()
        {
            return rules;
        }
    }
}
