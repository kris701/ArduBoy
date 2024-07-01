namespace ArduBoy.Compiler.Models.Script.Expressions.Arithmetic
{
    public class DivExp : BaseArithmeticExp
    {
        public IExp Value { get; set; }

        public DivExp(string name, IExp value) : base(name)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(:div {Name} {Value})";
        }
    }
}
