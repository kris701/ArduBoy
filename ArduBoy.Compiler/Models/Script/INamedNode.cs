namespace ArduBoy.Compiler.Models.Script
{
	public interface INamedNode : INode
	{
		public string Name { get; set; }
	}
}
