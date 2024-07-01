using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Expressions.Drawing
{
    public class DrawFillTriangleExp : DrawTriangleExp
    {
        public DrawFillTriangleExp(IExp w, IExp y1, IExp y2, IExp x1, IExp x2, IExp z, IExp color) : base(w, y1, y2, x1, x2, z, color)
        {
        }

        public override string ToString()
        {
            return $"(:draw-fill-triangle {X1} {Y1} {X2} {Y2} {X3} {Y3} {Color})";
        }
    }
}
