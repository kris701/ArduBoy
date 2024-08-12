namespace ArduBoy.Compiler.Models.Script.Expressions.Drawing
{
    public class DrawTriangleExp : BaseDraw
    {
        public IExp X1 { get; set; }
        public IExp Y1 { get; set; }
        public IExp X2 { get; set; }
        public IExp Y2 { get; set; }
        public IExp X3 { get; set; }
        public IExp Y3 { get; set; }

        public DrawTriangleExp(INode parent, IExp x1, IExp y1, IExp x2, IExp y2, IExp x3, IExp y3, IExp color) : base(parent, color)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            X3 = x3;
            Y3 = y3;
        }

        public override string ToString()
        {
            return $"(:draw-triangle {X1} {Y1} {X2} {Y2} {X3} {Y3} {Color})";
        }
    }
}
