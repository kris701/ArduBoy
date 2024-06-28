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
        public ValueExpression X1 { get; set; }
        public ValueExpression Y1 { get; set; }
        public ValueExpression X2 { get; set; }
        public ValueExpression Y2 { get; set; }

        public DrawLineExp(ValueExpression x1, ValueExpression y1, ValueExpression x2, ValueExpression y2, ValueExpression color) : base(color)
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
