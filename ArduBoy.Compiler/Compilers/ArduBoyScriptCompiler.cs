﻿using ArduBoy.Compiler.ASTGenerators;
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
			DoLog?.Invoke("Resetting parent structure...");
			from.SetParents();
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
					from.Statics = new StaticsDecl(new List<INode>());

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
			var fileStream = assembly.GetManifestResourceStream(resourceName);
			if (fileStream == null)
				throw new ArgumentNullException($"Cannot read resource: {target}");
			using (Stream stream = fileStream)
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
				if (setMap.TryGetValue(child.Name, out string? value))
					child.Name = value;
			}
		}

		private void InsertBasicGotos(ArduBoyScriptDefinition from)
		{
			var loop = from.Funcs.Single(x => x.Name.ToLower() == "loop");
			loop.Content.Add(new GotoExp(loop.Name));
			from.Funcs.Single(x => x.Name.ToLower() == "setup").Content.Add(new GotoExp(loop.Name));
		}

		private void DeconstructForLoops(ArduBoyScriptDefinition from)
		{
			var forNodes = from.FindTypes<ForExp>();

			var tmpID = 0;
			foreach (var node in forNodes)
			{
				if (node.Parent is IContentNode parent)
				{
					var newFunc = new FuncDecl($"for_{tmpID}", new List<INode>());
					newFunc.Content.AddRange(node.Content);
					newFunc.Content.Add(node.Updation);
					var endCondition = new IfExp(node.Condition, new List<INode>());
					endCondition.Content.Add(new GotoExp($"for_{tmpID}"));
					newFunc.Content.Add(endCondition);
					foreach (var subNode in newFunc.Content)
						subNode.Parent = newFunc;

					from.Funcs.Add(newFunc);

					var index = parent.Content.IndexOf(node);
					parent.Content.Remove(node);
					var replacement = new List<INode>();
					replacement.Add(new SetExp(node.Initialisation.Name, node.Initialisation.Value));
					replacement.Add(new CallExp($"for_{tmpID}"));

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
					var newFunc = new FuncDecl($"while_{tmpID}", new List<INode>());
					newFunc.Content.AddRange(node.Content);
					var endCondition = new IfExp(node.Condition, new List<INode>());
					endCondition.Content.Add(new GotoExp($"while_{tmpID}"));
					newFunc.Content.Add(endCondition);
					foreach (var subNode in newFunc.Content)
						subNode.Parent = newFunc;

					from.Funcs.Add(newFunc);

					var index = parent.Content.IndexOf(node);
					parent.Content.Remove(node);
					var replacement = new List<INode>();
					replacement.Add(new CallExp($"while_{tmpID}"));

					parent.Content.InsertRange(index, replacement);

					tmpID++;
				}
			}
		}
	}
}
