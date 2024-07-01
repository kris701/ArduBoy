namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class AudioExp : BaseNode, IExp
    {
        public IExp Value { get; set; }

        public AudioExp(IExp value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(:audio {Value})";
        }
    }
}
