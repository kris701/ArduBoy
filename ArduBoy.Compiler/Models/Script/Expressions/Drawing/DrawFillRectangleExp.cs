using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Expressions.Drawing
{
    public class DrawFillRectangleExp : DrawRectangleExp
    {
        public DrawFillRectangleExp(IExp x, IExp y, IExp width, IExp height, IExp color) : base(x, y, width, height, color)
        {
        }

        public override string ToString()
        {
            return $"(:draw-fill-rect {X} {Y} {Width} {Height} {Color})";
        }
    }
}
