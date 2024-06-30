using ArduBoy.Compiler.Models.Script.Expressions;

namespace ArduBoy.Compiler.Models.Script.Declarations
{
    public class IncludesDecl : BaseNode, IDecl
    {
        public List<IncludeExp> Includes { get; set; }

        public IncludesDecl(List<IncludeExp> statics)
        {
            Includes = statics;
        }

        public override string ToString()
        {
            return $"(:includes ({Includes.Count} more includes))";
        }
    }
}
