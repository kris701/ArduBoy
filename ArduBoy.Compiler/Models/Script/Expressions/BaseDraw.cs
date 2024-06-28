using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public abstract class BaseDraw : BaseNode, IExp
    {
        public ValueExpression Color { get; set; }

        protected BaseDraw(ValueExpression color)
        {
            Color = color;
        }
    }
}
