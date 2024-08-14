namespace ArduBoy.Compiler.Models.Script.Expressions
{
	public class WaitExp : BaseNode, IExp
	{
		public IExp WaitTime { get; set; }

		public WaitExp(IExp waitTime)
		{
			WaitTime = waitTime;
		}

		public override string ToString()
		{
			return $"WAIT {WaitTime}";
		}
	}
}
