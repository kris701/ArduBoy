using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Expressions.Arithmetic
{
    public class ModExp : BaseArithmeticExp
	{
		public IExp Value { get; set; }

		public ModExp(string name, IExp value) : base(name)
		{
			Value = value;
		}

		public override string ToString()
		{
			return $"(:mod {Name} {Value})";
		}
	}
}
