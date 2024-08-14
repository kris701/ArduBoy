namespace ArduBoy.Compiler.Models.Script.Expressions
{
	public class StaticsExp : BaseNode, IExp
	{
		public string Name { get; set; }
		public IExp Value { get; set; }

		public StaticsExp(string name, IExp value)
		{
			Name = name;
			Value = value;
		}

		public override string ToString()
		{
			return $"(:static {Name} {Value})";
		}
	}
}
