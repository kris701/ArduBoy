namespace ArduBoy.Compiler.Models.Script.Expressions.Arithmetic
{
    public class AddExp : BaseArithmeticExp
    {
        public IExp Value { get; set; }

        public AddExp(INode parent, string name, IExp value) : base(parent, name)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(:add {Name} {Value})";
        }
    }
}
