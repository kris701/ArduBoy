using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script
{
	public interface IContentNode
	{
		public List<INode> Content { get; set; }
	}
}
