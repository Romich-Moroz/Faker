using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace FakerLib.Configuration
{
    public class Rule
    {
        public Expression AssignOperation { get; private set; }
        public Type ParentClassType { get; private set; }
        public Type TargetFieldType { get; private set; }
        public Type FieldGeneratorType { get; private set; }
        

        public Rule(Type parentClass, Type fieldType, Type generatorType, Expression operation)
        {
            this.ParentClassType = parentClass;
            this.TargetFieldType = fieldType;
            this.FieldGeneratorType = generatorType;
            this.AssignOperation = operation;
        }

    }
}
