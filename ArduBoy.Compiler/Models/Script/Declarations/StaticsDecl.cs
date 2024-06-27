using ArduBoy.Compiler.Models.Script.Expressions;

namespace ArduBoy.Compiler.Models.Script.Declarations
{
    public class StaticsDecl : BaseNode, IDecl
    {
        public List<StaticsExp> Statics { get; set; }

        public StaticsDecl(List<StaticsExp> statics)
        {
            Statics = statics;
        }

        public override string ToString()
        {
            return $"(:statics ({Statics.Count} more statics))";
        }
    }
}
