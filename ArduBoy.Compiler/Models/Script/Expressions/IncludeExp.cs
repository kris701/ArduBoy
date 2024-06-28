using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class IncludeExp : BaseNode, IExp
    {
        public string Name { get; set; }

        public IncludeExp(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"(:include {Name})";
        }
    }
}
