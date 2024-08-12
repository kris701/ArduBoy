namespace ArduBoy.Compiler.Models.Script.Expressions.Drawing
{
    public class DrawFillTriangleExp : DrawTriangleExp
    {
        public DrawFillTriangleExp(INode parent, IExp x1, IExp y1, IExp x2, IExp y2, IExp x3, IExp y3, IExp color) : base(parent, x1, y1, x2, y2, x3, y3, color)
        {
        }

        public override string ToString()
        {
            return $"(:draw-fill-triangle {X1} {Y1} {X2} {Y2} {X3} {Y3} {Color})";
        }
    }
}
