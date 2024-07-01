namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class MultExp : BaseNode, IExp, INamedNode
    {
        public string Name { get; set; }
        public IExp Value { get; set; }

        public MultExp(string name, IExp value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"(:mult {Name} {Value})";
        }
    }
}
