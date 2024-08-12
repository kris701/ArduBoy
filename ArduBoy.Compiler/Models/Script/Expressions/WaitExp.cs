namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class WaitExp : BaseNode, IExp
    {
        public IExp WaitTime { get; set; }

        public WaitExp(INode parent, IExp waitTime) : base(parent)
        {
            WaitTime = waitTime;
        }

        public override string ToString()
        {
            return $"WAIT {WaitTime}";
        }
    }
}
