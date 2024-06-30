using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public abstract class BaseDraw : BaseNode, IExp
    {
        public IExp Color { get; set; }

        protected BaseDraw(IExp color)
        {
            Color = color;
        }
    }
}
