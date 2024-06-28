using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public abstract class BaseDraw : BaseNode, IExp
    {
        public enum DrawColor { Green, Blue, Red }
        public DrawColor Color { get; set; }

        protected BaseDraw(DrawColor color)
        {
            Color = color;
        }

        public static DrawColor GetColorFromString(string from)
        {
            switch (from.ToLower())
            {
                case "green": return DrawColor.Green;
                case "blue": return DrawColor.Blue;
                case "red": return DrawColor.Red;
            }
            throw new Exception($"Unknown color: {from}");
        }
    }
}
