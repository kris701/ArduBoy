namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class WaitExp : BaseNode, IExp
    {
        public int WaitTime { get; set; }

        public WaitExp(int waitTime)
        {
            WaitTime = waitTime;
        }

        public override string ToString()
        {
            return $"WAIT {WaitTime}";
        }
    }
}
