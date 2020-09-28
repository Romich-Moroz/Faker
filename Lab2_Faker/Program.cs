using System;
using System.Collections.Generic;
using FakerLib;
using FakerLib.Configuration;

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
            public Circular1 c { get; set; }
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

        struct SimpleStruct
        {
            public string field1;
            char field2;         
        }
        class PrivSetClass
        {
            public int prop { get; }
            public long prop2 { get; set; }

            public PrivSetClass(int prop)
            {
                this.prop = prop;
            }
        }
        struct PubCtorStruct
        {
            public int field1;
            char field2;

            public PubCtorStruct(char f)
            {
                field2 = f;
                field1 = 1;
            }
        }
        static void Main(string[] args)
        {
            var fakerConfig = new FakerConfig();
            fakerConfig.AddRule<StringGenClass, string, CustomStringGenerator>(c => c.s2);
            fakerConfig.AddRule<SimpleStruct, string, CustomStringGenerator>(c => c.field1);
            Faker f2 = new Faker(fakerConfig);
            f2.MaxCircularDependencyDepth = 1;
            var exp1 = f2.Create<Circular1>();
            var exp2 = f2.Create<NestedClass>();
            var exp3 = f2.Create<PubCtorStruct>();
            var exp4 = f2.Create<StringGenClass>();
            var exp5 = f2.Create<SimpleStruct>();
            var exp6 = f2.Create<PrivSetClass>();
            var exp7 = f2.Create<int>();
            var exp8 = f2.Create<DateTime>();
            var exp9 = f2.Create<string>();
        }

    }
}
