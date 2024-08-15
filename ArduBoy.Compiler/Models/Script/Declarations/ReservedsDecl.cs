using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Declarations
{
	public class ReservedsDecl : BaseNode, IDecl, IContentNode
	{
		public List<INode> Content { get; set; }

		public ReservedsDecl(List<INode> content)
		{
			Content = content;
		}

		public override string ToString()
		{
			return $"(:reserveds ({Content.Count} more reserveds))";
		}
	}
}
