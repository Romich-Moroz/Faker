using System;
using FakerLib;

namespace Lab2_Faker
{
    class Program
    {
        static void Main(string[] args)
        {
            Faker f = new Faker();
            Faker.Notify += Console.WriteLine;
            



            Faker.Notify -= Console.WriteLine;
        }

    }
}
