using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Expressions.Drawing
{
    public class DrawFillCircleExp : DrawCircleExp
    {
        public DrawFillCircleExp(IExp x, IExp y, IExp radius, IExp color) : base(x, y, radius, color)
        {
        }
        public override string ToString()
        {
            return $"(:draw-fill-circle {X} {Y} {Radius} {Color})";
        }

    }
}
