namespace ArduBoy.Compiler.Models.Script.Expressions.Drawing
{
    public abstract class BaseDraw : BaseNode, IExp
    {
        public IExp Color { get; set; }

        protected BaseDraw(INode parent, IExp color) : base(parent)
        {
            Color = color;
        }
    }
}
