using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class SetExp : BaseNode, IExp
    {
        public string Name { get; set; }
        public ValueExpression Value { get; set; }

        public SetExp(string name, ValueExpression value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"(:set {Name} {Value})";
        }
    }
}
