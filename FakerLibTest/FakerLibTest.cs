using FakerLib;
using FakerLib.Configuration;
using FakerLib.PluginSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace FakerLibTest
{
    [TestClass]
    public class FakerLibTest
    {
        private static Faker faker;
        

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            FakerConfig fc = new FakerConfig();
            fc.AddRule<PrivSetClass, int, CustomIntGen>(c => c.prop);
            faker = new Faker(fc);
        }
        struct DefCtorStruct
        {
            public int field1;
            char field2;
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
        struct PrivCtorStruct
        {
            public int field1;
            char field2;

            PrivCtorStruct(char f)
            {
                field2 = f;
                field1 = 1;
            }
        }
        struct MultCtorStruct
        {
            int field1;
            char field2;
            public float field3;

            public MultCtorStruct(int field)
            {
                field1 = field;
                field2 = '1';
                field3 = 0.1f;
            }

            public MultCtorStruct(int field1, char field2, float field3)
            {
                this.field1 = field1;
                this.field2 = field2;
                this.field3 = field3;
            }
        }

        class DefCtorClass
        {
            public int i;
            float f;
            public string s;
            public DateTime t;
            long l;
            public bool b;
            double d;
        }
        class PrivCtorClass
        {
            public int i;
            float f;
            public string s;
            public DateTime t;
            long l;
            public char c;
            public bool b;
            double d;

            PrivCtorClass(float f, long l)
            {
                this.f = f;
                this.l = l;
            }
        }

        class MultCtorClass
        {
            public int i;
            float f;
            public string s { get; set; }
            public DateTime t;
            long l;
            public char c { get; }
            public bool b { get; }
            double d;

            public MultCtorClass()
            {

            }
            public MultCtorClass(long l, double d)
            {
                this.l = l;
                this.d = d;
            }
        }

        class CollClass
        {
            public List<int> ints;
            List<double> doubles;
            List<char> chars;
            public List<DateTime> times;

            public CollClass(List<double> doubles)
            {
                this.doubles = doubles;
            }
        }

        class NestClass
        {
            public int a;
            public List<string> s;
            public CollClass c;
            CollClass pc;

            NestClass(CollClass pc)
            {
                this.pc = pc;
            }
            public NestClass()
            {

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

        class PrivSetClass
        {
            public int prop { get; }
            public long prop2 { get; set; }

            public PrivSetClass(int prop)
            {
                this.prop = prop;
            }
        }

        class CustomIntGen : Generator<int>
        {
            public override int Generate()
            {
                return 5;
            }
        }


        [TestMethod]
        public void CreateDefCtorStruct()
        {
            var actual = faker.Create<DefCtorStruct>();
            var notExpected = new DefCtorStruct();
            Assert.AreNotEqual(notExpected.field1, actual.field1);
        }
        [TestMethod]
        public void CreatePubCtorStruct()
        {
            var actual = faker.Create<PubCtorStruct>();
            var notExpected = new PubCtorStruct('f');
            Assert.AreEqual(notExpected.field1, actual.field1);
            //Assert.AreNotEqual(notExpected, actual);            
        }
        [TestMethod]
        public void CreatePrivCtorStruct()
        {
            var actual = faker.Create<PrivCtorStruct>();
            var notExpected = new PrivCtorStruct();
            Assert.AreNotEqual(notExpected.field1, actual.field1);
        }
        [TestMethod]
        public void CreateMultCtorStruct()
        {
            var actual = faker.Create<MultCtorStruct>();
            var notExpected = new MultCtorStruct();
            Assert.AreNotEqual(notExpected.field3, actual.field3);
        }
        [TestMethod]
        public void CreateDefCtorClass()
        {
            var actual = faker.Create<DefCtorClass>();
            var notExpected = new DefCtorClass();
            Assert.AreNotEqual(notExpected.i, actual.i);
            Assert.AreNotEqual(notExpected.s, actual.s);
            Assert.AreNotEqual(notExpected.t, actual.t);
        }
        [TestMethod]
        public void CreatePrivCtorClass()
        {
            var actual = faker.Create<PrivCtorClass>();
            PrivCtorClass Expected = null;
            Assert.AreEqual(Expected, actual);
        }
        [TestMethod]
        public void CreateMultCtorClass()
        {
            var actual = faker.Create<MultCtorClass>();
            var notExpected = new MultCtorClass();
            Assert.AreEqual(notExpected.c, actual.c);
            Assert.AreNotEqual(notExpected.i, actual.i);
            Assert.AreNotEqual(notExpected.s, actual.s);
            Assert.AreNotEqual(notExpected.t, actual.t);
        }
        [TestMethod]
        public void CreateCollCtorClass()
        {
            var actual = faker.Create<CollClass>();
            var notExpected = new CollClass(new List<double>());
            CollectionAssert.AreNotEqual(notExpected.ints, actual.ints);
            CollectionAssert.AreNotEqual(notExpected.times, actual.times);
        }
        [TestMethod]
        public void CreateNestClass()
        {
            var actual = faker.Create<NestClass>();
            var notExpected = new NestClass();
            Assert.AreNotEqual(notExpected.a, actual.a);
            Assert.AreNotEqual(notExpected.c, actual.c);
            CollectionAssert.AreNotEqual(notExpected.s, actual.s);
        }
        [TestMethod]
        public void CreateConfPrivSetClass()
        {
            var actual = faker.Create<PrivSetClass>();
            var Expected = new PrivSetClass(5);
            Assert.AreEqual(Expected.prop, actual.prop);
            Assert.AreNotEqual(Expected.prop2, actual.prop2);
        }

    }
}
