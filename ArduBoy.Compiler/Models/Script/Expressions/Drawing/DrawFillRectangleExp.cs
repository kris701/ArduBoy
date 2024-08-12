namespace ArduBoy.Compiler.Models.Script.Expressions.Drawing
{
    public class DrawFillRectangleExp : DrawRectangleExp
    {
        public DrawFillRectangleExp(INode parent, IExp x, IExp y, IExp width, IExp height, IExp color) : base(parent, x, y, width, height, color)
        {
        }

        public override string ToString()
        {
            return $"(:draw-fill-rect {X} {Y} {Width} {Height} {Color})";
        }
    }
}
