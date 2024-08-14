namespace ArduBoy.Compiler.Models.Script.Expressions
{
	public class GotoExp : BaseNode, IExp
	{
		public string Name { get; set; }

		public GotoExp(string target)
		{
			Name = target;
		}

		public override string ToString() => $"_goto {Name}";
	}
}
