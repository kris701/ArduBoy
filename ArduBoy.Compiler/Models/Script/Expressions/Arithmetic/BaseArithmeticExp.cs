namespace ArduBoy.Compiler.Models.Script.Expressions.Arithmetic
{
	public class BaseArithmeticExp : BaseNode, IExp, INamedNode, IArithmeticExp
	{
		public string Name { get; set; }

		public BaseArithmeticExp(string name)
		{
			Name = name;
		}
	}
}
