using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class AudioExp : BaseNode, IExp
    {
        public int Value { get; set; }

        public AudioExp(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(:audio {Value})";
        }
    }
}
