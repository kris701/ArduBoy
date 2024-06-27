namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class ComparisonExp : BaseNode, IExp
    {
        public ValueExpression Left { get; set; }
        public ValueExpression Right { get; set; }
        public string Type { get; set; }

        public ComparisonExp(ValueExpression left, ValueExpression right, string type)
        {
            Left = left;
            Right = right;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Left} {Type} {Right}";
        }
    }
}
