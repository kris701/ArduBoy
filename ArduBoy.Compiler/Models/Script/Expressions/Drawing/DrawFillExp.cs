using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Expressions.Drawing
{
    public class DrawFillExp : BaseDraw
    {
        public DrawFillExp(IExp color) : base(color)
        {
        }

        public override string ToString()
        {
            return $"(:draw-fill {Color})";
        }
    }
}
