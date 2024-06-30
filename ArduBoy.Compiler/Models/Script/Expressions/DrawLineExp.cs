﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class DrawLineExp : BaseDraw
    {
        public IExp X1 { get; set; }
        public IExp Y1 { get; set; }
        public IExp X2 { get; set; }
        public IExp Y2 { get; set; }

        public DrawLineExp(IExp x1, IExp y1, IExp x2, IExp y2, IExp color) : base(color)
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
