namespace ArduBoy.Compiler.Models.Script.Declarations
{
	public class StaticsDecl : BaseNode, IDecl, IContentNode
	{
		public List<INode> Content { get; set; }

		public StaticsDecl(List<INode> content)
		{
			Content = content;
		}

		public override string ToString()
		{
			return $"(:statics ({Content.Count} more statics))";
		}
	}
}
