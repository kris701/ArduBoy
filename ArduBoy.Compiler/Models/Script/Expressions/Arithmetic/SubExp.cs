namespace ArduBoy.Compiler.Models.Script.Expressions.Arithmetic
{
    public class SubExp : BaseArithmeticExp
    {
        public IExp Value { get; set; }

        public SubExp(string name, IExp value) : base(name)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(:sub {Name} {Value})";
        }
    }
}
