namespace ArduBoy.Compiler.Models.Script.Expressions.Arithmetic
{
    public class DivExp : BaseArithmeticExp
    {
        public IExp Value { get; set; }

        public DivExp(INode parent, string name, IExp value) : base(parent, name)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(:div {Name} {Value})";
        }
    }
}
