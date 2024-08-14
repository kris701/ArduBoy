namespace ArduBoy.Compiler.Models.Script.Declarations
{
	public class IncludesDecl : BaseNode, IDecl, IContentNode
	{
		public List<INode> Content { get; set; }

		public IncludesDecl(List<INode> content)
		{
			Content = content;
		}

		public override string ToString()
		{
			return $"(:includes ({Content.Count} more includes))";
		}
	}
}
