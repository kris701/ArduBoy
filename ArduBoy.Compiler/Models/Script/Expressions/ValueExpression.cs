namespace ArduBoy.Compiler.Models.Script.Expressions
{
	public class ValueExpression : BaseNode, IExp
	{
		public string Value { get; set; }

		public ValueExpression(string value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return Value;
		}
	}
}
