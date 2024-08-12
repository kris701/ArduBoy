namespace ArduBoy.Compiler.Models.Script.Expressions.Drawing
{
    public class DrawFillCircleExp : DrawCircleExp
    {
        public DrawFillCircleExp(INode parent, IExp x, IExp y, IExp radius, IExp color) : base(parent, x, y, radius, color)
        {
        }
        public override string ToString()
        {
            return $"(:draw-fill-circle {X} {Y} {Radius} {Color})";
        }

    }
}
