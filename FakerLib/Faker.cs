using FakerLib.PluginSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FakerLib
{
    public class Faker
    {

        public delegate void LogHandler(string message);
        public static event LogHandler Notify;

        private static Dictionary<Type, IGenerator> generators = LoadAllAvailableGenerators();

        /*public T Create<T>() 
        {
            T instance = (T)Activator.CreateInstance(typeof(T));
            foreach (FieldInfo fInfo in instance.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {

                dynamic generator =  
                fInfo.SetValue()
            }
            
        }
        public T Create<T>(FakerConfig config)
        {

        }

        private object SelectGenerator(Type fieldType)
        {
            switch(fieldType)
            {
                case Int:
            }
        }*/

        private static Dictionary<Type, IGenerator> LoadAllAvailableGenerators()
        {
            Dictionary<Type, IGenerator> result = new Dictionary<Type, IGenerator>();
            string pluginsPath = Directory.GetCurrentDirectory() + "\\Plugins\\";
            if (!Directory.Exists(pluginsPath))
            {
                Directory.CreateDirectory(pluginsPath);
            }
            foreach (string str in Directory.GetFiles(pluginsPath, "*.dll"))
            {
                try
                {
                    Assembly asm = Assembly.LoadFrom(str);
                    Type plugin = asm.GetTypes().Single(t => t.BaseType == typeof(GeneratorPlugin<>));
                    result.Add(plugin, (IGenerator)Activator.CreateInstance(plugin));
                }
                catch(Exception e)
                {
                    Notify?.Invoke(e.Message);
                }
            }
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType == typeof(GeneratorPlugin<>)))
            {
                result.Add(t, (IGenerator)Activator.CreateInstance(t));
            }
            return result;

        }

        public Faker()
        {
            
        }
    }
}
