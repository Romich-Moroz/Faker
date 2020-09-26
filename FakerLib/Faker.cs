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

        public int MaxCircularDependencyDepth { get; set; } = 5;

        private int currentCircularDependencyDepth = 0;

        private Dictionary<Type, IGenerator> generators;

        private Stack<Type> constructionStack = new Stack<Type>();

        public T Create<T>() 
        {
            ConstructorInfo[] constructors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.Public);

            if (constructors.Length == 0)
            {
                return default;
            }
            if ((currentCircularDependencyDepth = constructionStack.Where(t => t.Equals(typeof(T))).Count()) > MaxCircularDependencyDepth)
            {
                return default;
            }
            constructionStack.Push(typeof(T));

            foreach (ConstructorInfo cInfo in constructors.OrderByDescending(c => c.GetParameters().Length))
            {
                ParameterInfo[] pInfo = cInfo.GetParameters();
                object[] ctorParams = new object[pInfo.Length];
                for (int i = 0; i < ctorParams.Length; i++)
                {
                    Type fieldType = pInfo[i].ParameterType;                   
                    try
                    {
                        if (!fieldType.IsGenericType)
                        {
                            ctorParams[i] = generators[fieldType].GetType().InvokeMember("Generate", BindingFlags.InvokeMethod |
                                                    BindingFlags.Instance | BindingFlags.Public, null, generators[fieldType], null);
                        }
                        else
                        {
                            Type[] tmp = fieldType.GetGenericArguments();
                            ctorParams[i] = generators[tmp[0]].GetType().InvokeMember("GenerateList", BindingFlags.InvokeMethod |
                                                    BindingFlags.Instance | BindingFlags.Public, null, generators[tmp[0]], null);
                        }
                    }
                    catch (KeyNotFoundException e)
                    {
                        if (!(fieldType.IsPrimitive || (fieldType == typeof(string)) || (fieldType== typeof(decimal))))
                        {
                            ctorParams[i] = this.GetType().GetMethod("Create").MakeGenericMethod(fieldType).Invoke(this, null);
                        }
                        else
                        {
                            ctorParams[i] = default;
                        }
                        
                    }

                }
                object constructed;
                try
                {
                    constructed = cInfo.Invoke(ctorParams);
                }
                catch //constructor does not accept such params
                {
                    continue;
                }

                foreach (FieldInfo f in constructed.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
                {
                    bool contains = false;
                    for (int i = 0; i < ctorParams.Length; i++)
                    {
                        if (ctorParams[i]== f.GetValue(constructed) && f.FieldType == pInfo[i].ParameterType)
                        {
                            contains = true;
                        }
                    }

                    if (!contains)
                    {
                        Type fieldType = f.FieldType;
                        
                        try
                        {
                            if (!fieldType.IsGenericType)
                            {
                                f.SetValue(constructed, generators[fieldType].GetType().InvokeMember("Generate", BindingFlags.InvokeMethod |
                                                                BindingFlags.Instance | BindingFlags.Public, null, generators[fieldType], null));
                            }
                            else
                            {
                                Type[] tmp = fieldType.GetGenericArguments();
                                f.SetValue(constructed, generators[tmp[0]].GetType().InvokeMember("GenerateList", BindingFlags.InvokeMethod |
                                                                BindingFlags.Instance | BindingFlags.Public, null, generators[tmp[0]], null));
                            }
                        }
                        catch (KeyNotFoundException e)
                        {
                            if (!(fieldType.IsPrimitive || (fieldType == typeof(string)) || (fieldType == typeof(decimal))))
                            {
                                f.SetValue(constructed, this.GetType().GetMethod("Create").MakeGenericMethod(fieldType).Invoke(this, null));
                            }
                            else
                            {
                                f.SetValue(constructed, default); // if no generator exist then assign default value
                            }                           
                        }
                    }
                }
                constructionStack.Pop();
                return (T)constructed;
            }
            constructionStack.Pop();
            return default;
        }

        public Faker()
        {
            this.generators = LoadAllAvailableGenerators();
        }

        private Dictionary<Type, IGenerator> LoadAllAvailableGenerators()
        {
            Dictionary<Type, IGenerator> result = new Dictionary<Type, IGenerator>();
            string pluginsPath = Directory.GetCurrentDirectory() + "\\FakerLib Plugins\\Generators\\";
            if (!Directory.Exists(pluginsPath))
            {
                Directory.CreateDirectory(pluginsPath);
            }
            foreach (string str in Directory.GetFiles(pluginsPath, "*.dll"))
            {
                Assembly asm = Assembly.LoadFrom(str);
                foreach (Type t in asm.GetTypes())
                {
                    if (IsSupportedPlugin(t, typeof(GeneratorPlugin<>)))
                    {
                        var tmp = Activator.CreateInstance(t);
                        result.Add(t.BaseType.GetGenericArguments()[0], (IGenerator)tmp);

                    }
                }
            }
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (IsSupportedPlugin(t, typeof(Generator<>)))
                {
                    result.Add(t.BaseType.GetGenericArguments()[0], (IGenerator)Activator.CreateInstance(t));
                }              
            }
            return result;

        }

        private bool IsSupportedPlugin(Type plugin, Type required)
        {
            while (plugin != null && plugin != typeof(object))
            {
                Type tmp = plugin.IsGenericType ? plugin.GetGenericTypeDefinition() : plugin;
                if (required == tmp)
                {
                    return true;
                }
                plugin = plugin.BaseType;
            }
            return false;
        }

        
    }
}
