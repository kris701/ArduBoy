namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class ValueExpression : BaseNode, IExp
    {
        public string Value { get; set; }

        public ValueExpression(INode parent, string value) : base(parent)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
