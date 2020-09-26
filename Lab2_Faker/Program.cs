using System;
using System.Collections.Generic;
using FakerLib;
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
            public Circular2 c;
        }
        class Circular2
        {
            public Circular3 c;
        }
        class Circular3
        {
            public Circular1 c;
        }

        static void Main(string[] args)
        {
            Faker f = new Faker();
            f.MaxCircularDependencyDepth = 2;
            var exp1 = f.Create<Circular1>();
            var exp2 = f.Create<NestedClass>();

        }

    }
}
