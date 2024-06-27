using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class ComparisonExp : BaseNode, IExp
    {
        public string Left { get; set; }
        public string Right { get; set; }
        public string Type { get; set; }

        public ComparisonExp(INode parent, string left, string right, string type) : base(parent)
        {
            Left = left;
            Right = right;
            Type = type;
        }
    }
}
