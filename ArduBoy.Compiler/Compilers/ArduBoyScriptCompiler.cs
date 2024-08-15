using ArduBoy.Compiler.ASTGenerators;
using ArduBoy.Compiler.Helpers;
using ArduBoy.Compiler.Models.Exceptions;
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
			DoLog?.Invoke("Merging includes...");
			InsertIncludes(from);
			DoLog?.Invoke("Resetting parent structure...");
			from.SetParents();
			DoLog?.Invoke("Resolving statics...");
			ResolveStatics(from);
			DoLog?.Invoke("Inserting statics...");
			InsertStatics(from);
			DoLog?.Invoke("Resolving reserveds...");
			ResolveRefereds(from);
			DoLog?.Invoke("Inserting reserveds...");
			InsertReserveds(from);
			DoLog?.Invoke("Deconstruct for loops...");
			DeconstructForLoops(from);
			DoLog?.Invoke("Deconstruct while loops...");
			DeconstructWhileLoops(from);
			DoLog?.Invoke("Inserting primary function gotos...");
			InsertBasicGotos(from);
			DoLog?.Invoke("Converting variables names to indexes...");
			ConvertVariablesToIndexes(from);
			DoLog?.Invoke("Resetting parent structure...");
			from.SetParents();
			return from;
		}

		private void CheckIfScriptAreValid(ArduBoyScriptDefinition from)
		{
			if (!from.Funcs.Any(x => x.Name.ToLower() == "setup"))
				throw new CompilerException(from, "No setup function given!");
			if (!from.Funcs.Any(x => x.Name.ToLower() == "loop"))
				throw new CompilerException(from, "No loop function given!");
			if (from.Includes != null)
			{
				foreach (var node in from.Includes.Content)
				{
					if (node is IncludeExp include)
					{
						var assembly = Assembly.GetExecutingAssembly();
						var resourceName = $"ArduBoy.Compiler.CoreIncludes.{include.Name}.abs";
						if (assembly.GetManifestResourceStream(resourceName) == null)
							throw new CompilerException(include, $"Unknown include: {include.Name}");
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
				if (from.Reserveds == null)
					from.Reserveds = new ReservedsDecl(new List<INode>());

				foreach (var node in from.Includes.Content)
				{
					if (node is IncludeExp include)
					{
						var astGenerator = new ArduBoyScriptASTGenerator();
						var parser = new ArduBoyScriptParser();
						var ast = astGenerator.Generate(ResourceHelpers.ReadEmbeddedFile($"ArduBoy.Compiler.CoreIncludes.{include.Name}.abs"));
						var parsed = parser.Parse(ast);

						if (parsed.Statics != null)
							from.Statics.Content.AddRange(parsed.Statics.Content);
						if (parsed.Reserveds != null)
							from.Reserveds.Content.AddRange(parsed.Reserveds.Content);
						from.Funcs.AddRange(parsed.Funcs);
					}
				}
			}
		}

		private void ResolveStatics(ArduBoyScriptDefinition from)
		{
			var statics = from.FindTypes<StaticsExp>();
			while (statics.Any(x => x.Value is VariableExp))
			{
				foreach (var item in statics)
				{
					if (item.Value is VariableExp var)
					{
						if (!statics.Any(x => x.Name == var.Name))
							throw new CompilerException(item, "Statics referenced another static that does not exist!");
						var targetStatic = statics.First(x => x.Name == var.Name);
						if (targetStatic.Value is ValueExpression val)
							item.Replace(var, new ValueExpression(val.Value));
					}
				}
			}
		}

		private void InsertStatics(ArduBoyScriptDefinition from)
		{
			var statics = from.FindTypes<StaticsExp>();
			var variables = from.FindTypes<VariableExp>();
			foreach (var item in statics)
			{
				var refed = variables.Where(x => x.Name == item.Name);
				foreach(var refVar in refed)
					if (refVar.Parent != null && item.Value is ValueExpression val)
						refVar.Parent.Replace(refVar, new ValueExpression(val.Value));
			}
		}

		private void ResolveRefereds(ArduBoyScriptDefinition from)
		{
			var reserveds = from.FindTypes<ReservedExp>();
			while (reserveds.Any(x => x.ID is VariableExp))
			{
				foreach (var item in reserveds)
				{
					if (item.ID is VariableExp var)
					{
						if (!reserveds.Any(x => x.Name == var.Name))
							throw new CompilerException(item, "Reserved referenced another reserved that does not exist!");
						var targetStatic = reserveds.First(x => x.Name == var.Name);
						if (targetStatic.ID is ValueExpression val)
							item.Replace(var, new ValueExpression(val.Value));
					}
				}
			}
		}

		private void InsertReserveds(ArduBoyScriptDefinition from)
		{
			var reserveds = from.FindTypes<ReservedExp>();
			var variables = from.FindTypes<VariableExp>();
			foreach (var item in reserveds)
			{
				var refed = variables.Where(x => x.Name == item.Name);
				foreach (var refVar in refed)
					if (refVar.Parent != null && item.ID is ValueExpression val)
						refVar.Parent.Replace(refVar, new ValueExpression($"|{val.Value}"));
			}
		}

		private void ConvertVariablesToIndexes(ArduBoyScriptDefinition from)
		{
			var setMap = new Dictionary<string, string>();
			var sets = from.FindTypes<SetExp>();
			var counter = 0;
			foreach (var set in sets)
				if (!setMap.ContainsKey(set.Name))
					setMap.Add(set.Name, $"{counter++}");

			var all = from.FindTypes<INamedNode>();
			foreach (var child in all)
				if (setMap.TryGetValue(child.Name, out string? value))
					child.Name = value;
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
