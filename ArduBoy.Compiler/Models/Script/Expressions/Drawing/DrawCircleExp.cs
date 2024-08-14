namespace ArduBoy.Compiler.Models.Script.Expressions.Drawing
{
	public class DrawCircleExp : BaseDraw
	{
		public IExp X { get; set; }
		public IExp Y { get; set; }
		public IExp Radius { get; set; }

		public DrawCircleExp(IExp x, IExp y, IExp radius, IExp color) : base(color)
		{
			X = x;
			Y = y;
			Radius = radius;
		}

		public override string ToString()
		{
			return $"(:draw-circle {X} {Y} {Radius} {Color})";
		}
	}
}
