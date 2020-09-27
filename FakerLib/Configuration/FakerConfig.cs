using FakerLib.PluginSupport;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FakerLib.Configuration
{
    public class FakerConfig
    {
        private List<Rule> rules = new List<Rule>();

        public void AddRule<T, K, G>(Expression<Func<T,K>> exp) where G : Generator<K>
        {
            rules.Add(new Rule(typeof(T), typeof(K), typeof(G), ((MemberExpression)exp.Body).Member.Name));
        }

        public List<Rule> GetCreationRules()
        {
            return rules;
        }
    }
}
