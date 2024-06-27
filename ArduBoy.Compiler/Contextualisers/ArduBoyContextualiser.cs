using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Declarations;
using ArduBoy.Compiler.Models.Script.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Contextualisers
{
    public class ArduBoyContextualiser : IContextualiser
    {
        public ArduBoyScriptDefinition Contextualise(ArduBoyScriptDefinition from)
        {
            InsertDefines(from);
            SimplifyGotoLabels(from);
            return from;
        }

        private void InsertDefines(ArduBoyScriptDefinition from)
        {
            var defineDict = new Dictionary<string, ValueExpression>();
            foreach (var child in from.Nodes)
                if (child is StaticDefineNode def)
                    defineDict.Add(def.Name, def.As);

            foreach(var child in from.Nodes)
            {
                if (child is not StaticDefineNode)
                {
                    var all = child.FindTypes<ValueExpression>();
                    foreach (var value in all)
                        if (value.Reference == null && defineDict.Any(x => x.Key == value.Value))
                            value.Reference = defineDict.Single(x => x.Key == value.Value).Value;
                }
            }
        }

        private void SimplifyGotoLabels(ArduBoyScriptDefinition from)
        {
            var gotoDict = new Dictionary<string, string>();
            char count = 'a';
            foreach (var child in from.Nodes)
            {
                var all = child.FindTypes<GotoNode>();
                foreach (var value in all)
                    if (!gotoDict.ContainsKey(value.To))
                        gotoDict.Add(value.To, $"{count++}");
                var all2 = child.FindTypes<GotoLabelNode>();
                foreach (var value in all2)
                    if (!gotoDict.ContainsKey(value.Label))
                        gotoDict.Add(value.Label, $"{count++}");
            }

            foreach (var child in from.Nodes)
            {
                var all = child.FindTypes<GotoNode>();
                foreach (var value in all)
                    value.To = gotoDict[value.To];
                var all2 = child.FindTypes<GotoLabelNode>();
                foreach (var value in all2)
                    value.Label = gotoDict[value.Label];
            }
        }
    }
}
