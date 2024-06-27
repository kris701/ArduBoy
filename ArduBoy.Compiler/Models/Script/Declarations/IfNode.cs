using ArduBoy.Compiler.Models.Script.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Declarations
{
    public class IfNode : BaseNode, IDecl
    {
        public ComparisonExp Expression { get; set; }
        public List<INode> Branch { get; set; }

        public IfNode(INode parent, ComparisonExp expression, List<INode> branch) : base(parent)
        {
            Expression = expression;
            Branch = branch;
        }
    }
}
