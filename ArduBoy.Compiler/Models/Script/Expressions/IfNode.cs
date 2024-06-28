namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class IfNode : BaseNode, IExp
    {
        public ComparisonExp Expression { get; set; }
        public List<IExp> Content { get; set; }

        public IfNode(ComparisonExp expression, List<IExp> branch)
        {
            Expression = expression;
            Content = branch;
        }

        public override string ToString()
        {
            return $"(:if ({Expression}) ({Content.Count} contents))";
        }
    }
}
