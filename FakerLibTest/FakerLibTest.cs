using FakerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace FakerLibTest
{
    [TestClass]
    public class FakerLibTest
    {
        private static Faker basicFaker;
        

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            basicFaker = new Faker();

        }
        struct SimpleStruct
        {
            public int field1;
            char field2;
        }
        struct SimpleMultipleCtorStruct
        {
            int field1;
            char field2;
            public float field3;

            SimpleMultipleCtorStruct(int field)
            {
                field1 = field;
                field2 = '1';
                field3 = 0.1f;
            }

            SimpleMultipleCtorStruct(int field1, char field2, float field3)
            {
                this.field1 = field1;
                this.field2 = field2;
                this.field3 = field3;
            }
        }

        class SimpleClass
        {
            public int i;
            float f;
            public string s;
            public DateTime t;
            long l;
            public char c;
            public bool b;
            double d;
        }

        class MultipleCtorClass
        {
            public int i;
            float f;
            public string s;
            public DateTime t;
            long l;
            public char c;
            public bool b;
            double d;

            MultipleCtorClass()
            {

            }
            MultipleCtorClass(long l, double d)
            {
                this.l = l;
                this.d = d;
            }
        }

        class ParamCtorClass
        {
            public int i;
            double d;
            char c;

            ParamCtorClass(double d)
            {
                this.d = d;
            }
        }

        class PrivateCtorClass
        {
            int i;
            double d;
            char c;

            private PrivateCtorClass(double d)
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
            public List<string> s;
            public CollectionClass c;
            ParamCtorClass pc;

            NestedClass(ParamCtorClass pc)
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


        [TestMethod]
        public void CreateSimpleStruct()
        {
            var actual = basicFaker.Create<SimpleStruct>();
            var notExpected = new SimpleStruct();
            Assert.AreNotEqual(notExpected, actual);
        }
        [TestMethod]
        public void CreateSimpleMultipleCtorStruct()
        {
            var actual = basicFaker.Create<SimpleMultipleCtorStruct>();
            var notExpected = new SimpleMultipleCtorStruct();
            Assert.AreNotEqual(notExpected, actual);
        }
    }
}
