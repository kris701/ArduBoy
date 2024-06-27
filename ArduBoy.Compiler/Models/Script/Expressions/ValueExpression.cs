namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class ValueExpression : BaseNode, IExp
    {
        private string _value;
        public string Value
        {
            get
            {
                if (Reference != null)
                    return Reference.Value;
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        public ValueExpression? Reference { get; set; }

        public ValueExpression(string value)
        {
            Value = value;
        }

        public ValueExpression(ValueExpression reference)
        {
            Reference = reference;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
