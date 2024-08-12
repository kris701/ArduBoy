namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class VariableExp : BaseNode, IExp, INamedNode
    {
        public string Name { get; set; }
        public bool IsStatic { get; set; }

        public VariableExp(INode parent, string name, bool isStatic) : base(parent)
        {
            Name = name;
            IsStatic = isStatic;
        }

        public override string ToString()
        {
            if (IsStatic)
                return $"{Name}";
            return $"%{Name}%";
        }
    }
}
