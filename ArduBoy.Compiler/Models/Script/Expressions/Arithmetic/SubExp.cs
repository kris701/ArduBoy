namespace ArduBoy.Compiler.Models.Script.Expressions.Arithmetic
{
    public class SubExp : BaseArithmeticExp
    {
        public IExp Value { get; set; }

        public SubExp(INode parent, string name, IExp value) : base(parent, name)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(:sub {Name} {Value})";
        }
    }
}
