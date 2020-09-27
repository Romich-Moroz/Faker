using FakerLib.Configuration;
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

        public int MaxCircularDependencyDepth { get; set; } = 0;

        private int currentCircularDependencyDepth = 0;

        private Dictionary<Type, IGenerator> generators;

        private Stack<Type> constructionStack = new Stack<Type>();

        private List<Rule> creationRules;

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

                object[] ctorParams = GenerateCtorParams(cInfo);


                object constructed;
                try
                {
                    constructed = cInfo.Invoke(ctorParams);
                }
                catch //constructor does not accept such params
                {
                    continue;
                }

                GenerateFieldsAndProperties(constructed, ctorParams, cInfo);
                
                constructionStack.Pop();
                return (T)constructed;
            }
            constructionStack.Pop();
            return default;
        }

        private void GenerateFieldsAndProperties(object constructed, object[] ctorParams, ConstructorInfo cInfo)
        {
            ParameterInfo[] pInfo = cInfo.GetParameters();
            var fields = constructed.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public).Cast<MemberInfo>();
            var properties = constructed.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Cast<MemberInfo>();
            var fieldsAndProperties = fields.Concat(properties);

            foreach(MemberInfo m in fieldsAndProperties)
            {
                bool contains = false;

                Type memberType = (m as FieldInfo)?.FieldType ?? (m as PropertyInfo)?.PropertyType;
                object memberValue = (m as FieldInfo)?.GetValue(constructed) ?? (m as PropertyInfo)?.GetValue(constructed);

                for (int i = 0; i < ctorParams.Length; i++) 
                {
                    if (ctorParams[i] == memberValue && memberType == pInfo[i].ParameterType)
                    {
                        contains = true;
                        break;
                    }
                }
                if (!contains)
                {
                    object newValue = default; // if no generator exist then assign default value
                    try
                    {
                        
                        if (creationRules?.Any(r => (r.TargetFieldType == memberType) && (r.ParentClassType == cInfo.DeclaringType) && (r.FieldName == m.Name)) == true)
                        {
                            object gen = Activator.CreateInstance(creationRules.Single(r => (r.TargetFieldType == memberType) && (r.ParentClassType == cInfo.DeclaringType)
                                                                    && (r.FieldName == m.Name)).FieldGeneratorType);
                            newValue = gen.GetType().InvokeMember("Generate", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, gen, null);
                            
                        }
                        else
                        {
                            if (!memberType.IsGenericType)
                            {
                                newValue = generators[memberType].GetType().InvokeMember("Generate", BindingFlags.InvokeMethod | BindingFlags.Instance
                                                                                        | BindingFlags.Public, null, generators[memberType], null);
                            }
                            else
                            {
                                Type[] tmp = memberType.GetGenericArguments();
                                newValue = generators[tmp[0]].GetType().InvokeMember("GenerateList", BindingFlags.InvokeMethod | BindingFlags.Instance
                                                                                     | BindingFlags.Public, null, generators[tmp[0]], null);
                            }
                        }
                    }
                    catch (KeyNotFoundException e)
                    {
                        if (!(memberType.IsPrimitive || (memberType == typeof(string)) || (memberType == typeof(decimal))))
                        {
                            newValue = this.GetType().GetMethod("Create").MakeGenericMethod(memberType).Invoke(this, null);
                        }
                    }
                    (m as FieldInfo)?.SetValue(constructed, newValue);
                    if ((m as PropertyInfo)?.CanWrite == true)
                    {
                        (m as PropertyInfo).SetValue(constructed, newValue);
                    }
                }
            }

        }

        private object[] GenerateCtorParams(ConstructorInfo cInfo)
        {
            ParameterInfo[] pInfo = cInfo.GetParameters();
            object[] ctorParams = new object[pInfo.Length];

            for (int i = 0; i < ctorParams.Length; i++)
            {
                Type fieldType = pInfo[i].ParameterType;
                object newValue = default;
                try
                {
                    
                    if (creationRules?.Any(r => (r.TargetFieldType == fieldType) && (r.ParentClassType == cInfo.DeclaringType) && (r.FieldName == pInfo[i].Name)) == true)
                    {
                        object gen = Activator.CreateInstance(creationRules.Single(r => (r.TargetFieldType == fieldType) && (r.ParentClassType == cInfo.DeclaringType)
                                                                && (r.FieldName == pInfo[i].Name)).FieldGeneratorType);
                        newValue = gen.GetType().InvokeMember("Generate", BindingFlags.InvokeMethod |
                                                    BindingFlags.Instance | BindingFlags.Public, null, gen, null);
                    }
                    else
                    {
                        if (!fieldType.IsGenericType)
                        {

                            newValue = generators[fieldType].GetType().InvokeMember("Generate", BindingFlags.InvokeMethod |
                                                    BindingFlags.Instance | BindingFlags.Public, null, generators[fieldType], null);
                        }
                        else
                        {
                            Type[] tmp = fieldType.GetGenericArguments();
                            newValue = generators[tmp[0]].GetType().InvokeMember("GenerateList", BindingFlags.InvokeMethod |
                                                    BindingFlags.Instance | BindingFlags.Public, null, generators[tmp[0]], null);
                        }
                    }                    
                }
                catch (KeyNotFoundException e)
                {
                    if (!(fieldType.IsPrimitive || (fieldType == typeof(string)) || (fieldType == typeof(decimal))))
                    {
                        newValue = this.GetType().GetMethod("Create").MakeGenericMethod(fieldType).Invoke(this, null);
                    }

                }
                ctorParams[i] = newValue;
            }
            return ctorParams;
        }

        public Faker()
        {
            this.generators = LoadAllAvailableGenerators();
        }

        public Faker(FakerConfig config) : this()
        {
            creationRules = config.GetCreationRules();
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
                    if (IsRequiredType(t, typeof(GeneratorPlugin<>)))
                    {
                        var tmp = Activator.CreateInstance(t);
                        result.Add(t.BaseType.GetGenericArguments()[0], (IGenerator)tmp);

                    }
                }
            }
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (IsRequiredType(t, typeof(Generator<>)))
                {
                    result.Add(t.BaseType.GetGenericArguments()[0], (IGenerator)Activator.CreateInstance(t));
                }              
            }
            return result;

        }

        private bool IsRequiredType(Type plugin, Type required)
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
