namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class ComparisonExp : BaseNode, IExp
    {
        public IExp Left { get; set; }
        public IExp Right { get; set; }
        public string Operator { get; set; }

        public ComparisonExp(INode parent, IExp left, IExp right, string @operator) : base(parent)
        {
            Left = left;
            Right = right;
            Operator = @operator;
        }

        public override string ToString()
        {
            return $"{Left} {Operator} {Right}";
        }
    }
}
