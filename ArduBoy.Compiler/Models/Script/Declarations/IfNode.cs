using ArduBoy.Compiler.Models.Script.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ArduBoy.Compiler.Models.Script.Declarations
{
    public class IfNode : BaseNode, IDecl
    {
        public ComparisonExp Expression { get; set; }
        public List<INode> Branch { get; set; }

        public IfNode(ComparisonExp expression, List<INode> branch)
        {
            Expression = expression;
            Branch = branch;
        }

        public override string ToString()
        {
            return $"IF {Expression} ...";
        }
    }
}
