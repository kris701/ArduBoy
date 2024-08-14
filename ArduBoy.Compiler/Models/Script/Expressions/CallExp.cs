namespace ArduBoy.Compiler.Models.Script.Expressions
{
	public class CallExp : BaseNode, IExp, INamedNode
	{
		public string Name { get; set; }

		public CallExp(string name)
		{
			Name = name;
		}

		public override string ToString()
		{
			return $"(:call {Name})";
		}
	}
}
