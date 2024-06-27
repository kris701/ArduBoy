using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Expressions;

namespace ArduBoy.Compiler.Contextualisers
{
    public class ArduBoyContextualiser : IContextualiser
    {
        public ArduBoyScriptDefinition Contextualise(ArduBoyScriptDefinition from)
        {
            InsertDefines(from);
            return from;
        }

        private void InsertDefines(ArduBoyScriptDefinition from)
        {
            var defineDict = new Dictionary<string, ValueExpression>();
            if (from.Statics != null)
            {
                foreach (var child in from.Statics.Statics)
                    defineDict.Add(child.Name, child.Value);
            }

            var all = from.FindTypes<ValueExpression>();
            foreach (var child in all)
                if (child.Reference == null && defineDict.Any(x => x.Key == child.Value))
                    child.Reference = defineDict.Single(x => x.Key == child.Value).Value;
        }
    }
}
