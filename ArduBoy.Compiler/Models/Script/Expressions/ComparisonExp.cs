namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class ComparisonExp : BaseNode, IExp
    {
        public IExp Left { get; set; }
        public IExp Right { get; set; }
        public string Type { get; set; }

        public ComparisonExp(IExp left, IExp right, string type)
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
