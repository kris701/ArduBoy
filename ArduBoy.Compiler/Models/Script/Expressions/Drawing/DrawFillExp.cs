namespace ArduBoy.Compiler.Models.Script.Expressions.Drawing
{
    public class DrawFillExp : BaseDraw
    {
        public DrawFillExp(INode parent, IExp color) : base(parent, color)
        {
        }

        public override string ToString()
        {
            return $"(:draw-fill {Color})";
        }
    }
}
