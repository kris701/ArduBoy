namespace ArduBoy.Compiler.Models.Script.Expressions.Drawing
{
    public class DrawRectangleExp : BaseDraw
    {
        public IExp X { get; set; }
        public IExp Y { get; set; }
        public IExp Width { get; set; }
        public IExp Height { get; set; }

        public DrawRectangleExp(INode parent, IExp x, IExp y, IExp width, IExp height, IExp color) : base(parent, color)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"(:draw-rect {X} {Y} {Width} {Height} {Color})";
        }
    }
}
