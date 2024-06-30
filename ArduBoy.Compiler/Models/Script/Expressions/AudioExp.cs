namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class AudioExp : BaseNode, IExp
    {
        public int Value { get; set; }

        public AudioExp(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(:audio {Value})";
        }
    }
}
