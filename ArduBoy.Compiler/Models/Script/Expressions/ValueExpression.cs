namespace ArduBoy.Compiler.Models.Script.Expressions
{
	public class ValueExpression : BaseNode, IExp
	{
		public enum ValueTypes { Unknown, Integer, String }
		private string _value = "";
		public string Value
		{
			get => _value;
			set
			{
				_value = value;
				_type = ValueTypes.Unknown;
			}
		}
		private ValueTypes _type = ValueTypes.Unknown;

		public ValueExpression(string value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return Value;
		}

		public ValueTypes EvaluatedType()
		{
			if (_type != ValueTypes.Unknown)
				return _type;

			if (Value.StartsWith('|'))
				_type = ValueTypes.Integer;
			else if (int.TryParse(Value, out int tmp))
				_type = ValueTypes.Integer;
			else
				_type = ValueTypes.String;

			return _type;
		}
	}
}
