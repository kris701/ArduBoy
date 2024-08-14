namespace ArduBoy.Compiler.Models.Script.Expressions
{
	public class VariableExp : BaseNode, IExp, INamedNode
	{
		public string Name { get; set; }
		public bool IsStatic { get; set; }

		public VariableExp(string name, bool isStatic)
		{
			Name = name;
			IsStatic = isStatic;
		}

		public override string ToString()
		{
			if (IsStatic)
				return $"{Name}";
			return $"%{Name}%";
		}
	}
}
