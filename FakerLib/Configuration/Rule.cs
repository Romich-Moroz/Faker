using System;

namespace FakerLib.Configuration
{
    public class Rule
    {
        public string FieldName { get; private set; }
        public Type ParentClassType { get; private set; }
        public Type TargetFieldType { get; private set; }
        public Type FieldGeneratorType { get; private set; }
        

        public Rule(Type parentClass, Type fieldType, Type generatorType, string fieldName)
        {
            this.ParentClassType = parentClass;
            this.TargetFieldType = fieldType;
            this.FieldGeneratorType = generatorType;
            this.FieldName = fieldName;
        }

    }
}
