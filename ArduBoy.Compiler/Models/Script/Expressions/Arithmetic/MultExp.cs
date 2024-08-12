namespace ArduBoy.Compiler.Models.Script.Expressions.Arithmetic
{
    public class MultExp : BaseArithmeticExp
    {
        public IExp Value { get; set; }

        public MultExp(INode parent, string name, IExp value) : base(parent, name)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(:mult {Name} {Value})";
        }
    }
}
