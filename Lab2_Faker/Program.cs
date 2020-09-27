using System;
using System.Collections.Generic;
using FakerLib;
using FakerLib.Configuration;
using FakerLib.Generators;

namespace Lab2_Faker
{
    class Program
    {

        class ParamCtorClass
        {
            public string i;
            double d;
            char c;

            public ParamCtorClass(double d)
            {
                this.d = d;
            }
        }

        class CollectionClass
        {
            public List<int> ints;
            List<double> doubles;
            List<char> chars;

            public CollectionClass(List<double> doubles)
            {
                this.doubles = doubles;
            }
        }

        class NestedClass
        {
            public int a;
            public List<long> s;
            public CollectionClass c;
            ParamCtorClass pc;

            public NestedClass(ParamCtorClass pc)
            {
                this.pc = pc;
            }
        }
        class Circular1
        {
            public Circular2 c { get; set; }
        }
        class Circular2
        {
            public Circular3 c { get; set; }
        }
        class Circular3
        {
            public Circular1 c { get; }
        }

        class StringGenClass
        {
            public string s1;
            public string s2 { get; }
            public StringGenClass(string s2)
            {
                this.s2 = s2;
            }
        }

        static void Main(string[] args)
        {
            Faker f = new Faker();
            f.MaxCircularDependencyDepth = 2;
            var exp1 = f.Create<Circular1>();
            var exp2 = f.Create<NestedClass>();

            var fakerConfig = new FakerConfig();
            fakerConfig.AddRule<StringGenClass, string, CustomStringGenerator>(c => c.s2);
            fakerConfig.AddRule<StringGenClass, string, CustomStringGenerator>(c => c.s1);
            Faker f2 = new Faker(fakerConfig);

            var exp3 = f2.Create<StringGenClass>();
        }

    }
}
