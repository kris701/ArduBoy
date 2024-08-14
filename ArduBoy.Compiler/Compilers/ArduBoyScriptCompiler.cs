using ArduBoy.Compiler.ASTGenerators;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Declarations;
using ArduBoy.Compiler.Models.Script.Expressions;
using ArduBoy.Compiler.Parsers;
using System.Collections.Generic;
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

			DoLog?.Invoke("Deconstruct for loops...");
			DeconstructForLoops(from);
			DoLog?.Invoke("Deconstruct while loops...");
			DeconstructWhileLoops(from);
			DoLog?.Invoke("Inserting primary function gotos...");
            InsertBasicGotos(from);
            DoLog?.Invoke("Merging includes...");
            InsertIncludes(from);
            DoLog?.Invoke("Converting variables names to indexes...");
            ConvertVariablesToIndexes(from);
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
                foreach (var node in from.Includes.Content)
                {
                    if (node is IncludeExp include)
                    {
                        var assembly = Assembly.GetExecutingAssembly();
                        var resourceName = $"ArduBoy.Compiler.CoreIncludes.{include.Name}.abs";
                        if (assembly.GetManifestResourceStream(resourceName) == null)
                            throw new Exception($"Unknown include: {include.Name}");
                    }
                }
            }
        }

        private void InsertIncludes(ArduBoyScriptDefinition from)
        {
            if (from.Includes != null)
            {
                if (from.Statics == null)
                    from.Statics = new StaticsDecl(from, new List<INode>());

                foreach (var node in from.Includes.Content)
                {
                    if (node is IncludeExp include)
                    {
                        var astGenerator = new ArduBoyScriptASTGenerator();
                        var parser = new ArduBoyScriptParser();
                        var ast = astGenerator.Generate(ReadEmbeddedFile(include.Name));
                        var parsed = parser.Parse(ast);

                        if (parsed.Statics != null)
                            from.Statics.Content.AddRange(parsed.Statics.Content);
                        from.Funcs.AddRange(parsed.Funcs);
                    }
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
                if (!setMap.ContainsKey(item.Name) && item.Value is ValueExpression val)
                    setMap.Add(item.Name, val.Value);

            var all = from.FindTypes<INamedNode>();
            foreach (var child in all)
            {
                if (child is VariableExp exp && !setMap[child.Name].StartsWith('_') && statics.Any(x => x.Name == exp.Name))
                    exp.IsStatic = true;
                if (setMap.ContainsKey(child.Name))
                    child.Name = setMap[child.Name];
            }
        }

        private void InsertBasicGotos(ArduBoyScriptDefinition from)
        {
            var loop = from.Funcs.Single(x => x.Name.ToLower() == "loop");
            loop.Content.Add(new GotoExp(loop.Parent, loop.Name));
            from.Funcs.Single(x => x.Name.ToLower() == "setup").Content.Add(new GotoExp(loop.Parent, loop.Name));
        }

		private void DeconstructForLoops(ArduBoyScriptDefinition from)
		{
            var forNodes = from.FindTypes<ForExp>();

            var tmpID = 0; 
            foreach(var node in forNodes)
            {
                if (node.Parent is IContentNode parent)
                {
                    var newFunc = new FuncDecl(from, $"for_{tmpID}", new List<INode>());
					newFunc.Content.AddRange(node.Content);
					newFunc.Content.Add(node.Updation);
                    var endCondition = new IfNode(newFunc, node.Condition, new List<INode>());
                    endCondition.Content.Add(new GotoExp(newFunc, $"for_{tmpID}"));
					newFunc.Content.Add(endCondition);
					foreach (var subNode in newFunc.Content)
						subNode.Parent = newFunc;

					from.Funcs.Add(newFunc);

					var index = parent.Content.IndexOf(node);
					parent.Content.Remove(node);
					var replacement = new List<INode>();
					replacement.Add(new SetExp(node.Parent, node.Initialisation.Name, node.Initialisation.Value));
                    replacement.Add(new CallExp(node.Parent, $"for_{tmpID}"));

					parent.Content.InsertRange(index, replacement);

                    tmpID++;
				}
            }
		}

		private void DeconstructWhileLoops(ArduBoyScriptDefinition from)
		{
			var whileNodes = from.FindTypes<WhileExp>();

			var tmpID = 0;
			foreach (var node in whileNodes)
			{
				if (node.Parent is IContentNode parent)
				{
					var newFunc = new FuncDecl(from, $"while_{tmpID}", new List<INode>());
					newFunc.Content.AddRange(node.Content);
					var endCondition = new IfNode(newFunc, node.Condition, new List<INode>());
					endCondition.Content.Add(new GotoExp(newFunc, $"while_{tmpID}"));
					newFunc.Content.Add(endCondition);
					foreach (var subNode in newFunc.Content)
						subNode.Parent = newFunc;

					from.Funcs.Add(newFunc);

					var index = parent.Content.IndexOf(node);
					parent.Content.Remove(node);
					var replacement = new List<INode>();
					replacement.Add(new CallExp(node.Parent, $"while_{tmpID}"));

					parent.Content.InsertRange(index, replacement);

					tmpID++;
				}
			}
		}
	}
}
