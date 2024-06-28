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
            InsertDefines(from);
            return from;
        }

        private void InsertDefines(ArduBoyScriptDefinition from)
        {
            if (from.Statics != null)
            {
                var defineDict = new Dictionary<string, ValueExpression>();

                foreach (var child in from.Statics.Statics)
                    defineDict.Add(child.Name, child.Value);

                var all = from.FindTypes<ValueExpression>();
                foreach (var child in all)
                    if (child.Reference == null && defineDict.Any(x => x.Key == child.Value))
                        child.Reference = defineDict.Single(x => x.Key == child.Value).Value;
            }
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
    }
}
