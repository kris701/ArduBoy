using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Expressions
{
	public class ReservedExp : BaseNode, IExp
	{
		public string Name { get; set; }
		public IExp ID { get; set; }

		public ReservedExp(string name, IExp id)
		{
			Name = name;
			ID = id;
		}

		public override string ToString()
		{
			return $"(:reserved {Name} {ID})";
		}
	}
}
