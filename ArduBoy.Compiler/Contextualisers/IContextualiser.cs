using ArduBoy.Compiler.Models.Script;

namespace ArduBoy.Compiler.Contextualisers
{
    public interface IContextualiser
    {
        public ArduBoyScriptDefinition Contextualise(ArduBoyScriptDefinition from);
    }
}
