using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class DrawLineExp : BaseDraw
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }

        public DrawLineExp(int x1, int y1, int x2, int y2, DrawColor color) : base(color)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public override string ToString()
        {
            return $"(:draw-line {X1} {Y1} {X2} {Y2} {Color})";
        }
    }
}
