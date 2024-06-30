using ArduBoy.Compiler.ASTGenerators;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Declarations;
using ArduBoy.Compiler.Models.Script.Expressions;
using ArduBoy.Compiler.Parsers;
using System.Reflection;

namespace ArduBoy.Compiler.Compilers
{
    public class ArduBoyScriptCompiler : ICompiler
    {
        public event LogEventHandler? DoLog;

        public ArduBoyScriptDefinition Compile(ArduBoyScriptDefinition from)
        {
            DoLog?.Invoke("Checking if script is valid...");
            CheckIfScriptAreValid(from);

            DoLog?.Invoke("Inserting primary function gotos...");
            InsertBasicGotos(from);
            DoLog?.Invoke("Merging includes...");
            InsertIncludes(from);
            DoLog?.Invoke("Converting variables names to indexes...");
            ConvertVariablesToIndexes(from);
            DoLog?.Invoke("Converting function names to indexes...");
            ConvertFuncNamesToIndexes(from);
            return from;
        }

        private void CheckIfScriptAreValid(ArduBoyScriptDefinition from)
        {
            if (!from.Funcs.Any(x => x.Name.ToLower() == "setup"))
                throw new Exception("No setup function given!");
            if (!from.Funcs.Any(x => x.Name.ToLower() == "loop"))
                throw new Exception("No loop function given!");
            if (from.Includes != null)
            {
                foreach (var include in from.Includes.Includes)
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var resourceName = $"ArduBoy.Compiler.CoreIncludes.{include.Name}.abs";
                    if (assembly.GetManifestResourceStream(resourceName) == null)
                        throw new Exception($"Unknown include: {include.Name}");
                }
            }
        }

        private void InsertIncludes(ArduBoyScriptDefinition from)
        {
            if (from.Includes != null)
            {
                if (from.Statics == null)
                    from.Statics = new StaticsDecl(new List<StaticsExp>());

                foreach (var include in from.Includes.Includes)
                {
                    var astGenerator = new ArduBoyScriptASTGenerator();
                    var parser = new ArduBoyScriptParser();
                    var ast = astGenerator.Generate(ReadEmbeddedFile(include.Name));
                    var parsed = parser.Parse(ast);

                    if (parsed.Statics != null)
                        from.Statics.Statics.AddRange(parsed.Statics.Statics);
                    from.Funcs.AddRange(parsed.Funcs);
                }
            }
        }

        private string ReadEmbeddedFile(string target)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"ArduBoy.Compiler.CoreIncludes.{target}.abs";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
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
            foreach (var call in calls)
                if (setMap.ContainsKey(call.Name))
                    call.Name = setMap[call.Name];
            var gotos = from.FindTypes<GotoExp>();
            foreach (var @goto in gotos)
                if (setMap.ContainsKey(@goto.Target))
                    @goto.Target = setMap[@goto.Target];
        }

        private void InsertBasicGotos(ArduBoyScriptDefinition from)
        {
            var loop = from.Funcs.Single(x => x.Name.ToLower() == "loop");
            loop.Content.Add(new GotoExp(loop.Name));
            from.Funcs.Single(x => x.Name.ToLower() == "setup").Content.Add(new GotoExp(loop.Name));
        }
    }
}
