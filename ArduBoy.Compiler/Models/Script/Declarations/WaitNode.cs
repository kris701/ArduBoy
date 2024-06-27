namespace ArduBoy.Compiler.Models.Script.Declarations
{
    public class WaitNode : BaseNode, IDecl
    {
        public int WaitTime { get; set; }

        public WaitNode(int waitTime)
        {
            WaitTime = waitTime;
        }

        public override string ToString()
        {
            return $"WAIT {WaitTime}";
        }
    }
}
