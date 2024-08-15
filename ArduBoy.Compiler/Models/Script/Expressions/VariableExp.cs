namespace ArduBoy.Compiler.Models.Script.Expressions
{
	public class VariableExp : BaseNode, IExp, INamedNode
	{
		public string Name { get; set; }

		public VariableExp(string name)
		{
			Name = name;
		}

		public override string ToString()
		{
			return $"%{Name}%";
		}
	}
}
