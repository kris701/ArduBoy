namespace ArduBoy.Compiler.Models.Script.Expressions.Drawing
{
    public class DrawTextExp : BaseDraw
    {
        public IExp X { get; set; }
        public IExp Y { get; set; }
        public IExp Size { get; set; }
        public IExp Text { get; set; }

        public DrawTextExp(INode parent, IExp x, IExp y, IExp size, IExp text, IExp color) : base(parent, color)
        {
            X = x;
            Y = y;
            Size = size;
            Text = text;
        }

        public override string ToString()
        {
            return $"(:draw-text {X} {Y} {Size} {Color} {Text})";
        }
    }
}
