namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class AudioExp : BaseNode, IExp
    {
        public IExp Value { get; set; }

        public AudioExp(INode parent, IExp value) : base(parent)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(:audio {Value})";
        }
    }
}
