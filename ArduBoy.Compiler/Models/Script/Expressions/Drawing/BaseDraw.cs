namespace ArduBoy.Compiler.Models.Script.Expressions.Drawing
{
	public abstract class BaseDraw : BaseNode, IExp
	{
		public IExp Color { get; set; }

		protected BaseDraw(IExp color)
		{
			Color = color;
		}
	}
}
