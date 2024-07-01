using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Expressions.Drawing
{
    public class DrawRectangleExp : BaseDraw
    {
        public IExp X { get; set; }
        public IExp Y { get; set; }
        public IExp Width { get; set; }
        public IExp Height { get; set; }

        public DrawRectangleExp(IExp x, IExp y, IExp width, IExp height, IExp color) : base(color)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"(:draw-rect {X} {Y} {Width} {Height} {Color})";
        }
    }
}
