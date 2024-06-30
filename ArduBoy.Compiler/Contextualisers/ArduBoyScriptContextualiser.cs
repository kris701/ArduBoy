using ArduBoy.Compiler.ASTGenerators;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Declarations;
using ArduBoy.Compiler.Models.Script.Expressions;
using ArduBoy.Compiler.Parsers;
using System.Reflection;

namespace ArduBoy.Compiler.Contextualisers
{
    public class ArduBoyScriptContextualiser : IContextualiser
    {
        public ArduBoyScriptDefinition Contextualise(ArduBoyScriptDefinition from)
        {
            InsertIncludes(from);
            ConvertVariablesToIndexes(from);
            ConvertFuncNamesToIndexes(from);
            return from;
        }

        private void InsertIncludes(ArduBoyScriptDefinition from)
        {
            if (from.Includes != null)
            {
                if (from.Statics == null)
                    from.Statics = new StaticsDecl(new List<StaticsExp>());

                var assembly = Assembly.GetExecutingAssembly();
                foreach (var include in from.Includes.Includes)
                {
                    var resourceName = $"ArduBoy.Compiler.CoreIncludes.{include.Name}.abs";
                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        var astGenerator = new ArduBoyScriptASTGenerator();
                        var parser = new ArduBoyScriptParser();
                        var ast = astGenerator.Generate(reader.ReadToEnd());
                        var parsed = parser.Parse(ast);

                        if (parsed.Statics != null)
                            from.Statics.Statics.AddRange(parsed.Statics.Statics);
                        from.Funcs.AddRange(parsed.Funcs);
                    }
                }
            }
        }

        private void ConvertVariablesToIndexes(ArduBoyScriptDefinition from)
        {
            var setMap = new Dictionary<string, string>();
            var sets = from.FindTypes<SetExp>();
            var counter = 0;
            foreach (var set in sets)
            {
                if (!setMap.ContainsKey(set.Name))
                    setMap.Add(set.Name, $"{counter++}");
                set.Name = setMap[set.Name];
            }
            var statics = from.FindTypes<StaticsExp>();
            foreach (var item in statics)
            {
                if (!setMap.ContainsKey(item.Name))
                {
                    if (item.Value.Value.StartsWith('_'))
                        setMap.Add(item.Name, item.Value.Value);
                    else
                        setMap.Add(item.Name, $"{counter++}");
                }
                item.Name = setMap[item.Name];
            }

            var all = from.FindTypes<VariableExp>();
            foreach (var child in all)
                if (setMap.ContainsKey(child.Name))
                    child.Name = setMap[child.Name];
        }

        private void ConvertFuncNamesToIndexes(ArduBoyScriptDefinition from)
        {
            var setMap = new Dictionary<string, string>();
            var sets = from.FindTypes<FuncDecl>();
            var counter = 0;
            foreach (var set in sets)
            {
                if (!setMap.ContainsKey(set.Name))
                    setMap.Add(set.Name, $"{counter++}");
                set.Name = setMap[set.Name];
            }

            var calls = from.FindTypes<CallExp>();
            foreach(var call in calls)
                if (setMap.ContainsKey(call.Name))
                    call.Name = setMap[call.Name];
        }
    }
}
