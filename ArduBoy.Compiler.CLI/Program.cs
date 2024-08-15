using ArduBoy.Compiler.ASTGenerators;
using ArduBoy.Compiler.CodeGenerators;
using ArduBoy.Compiler.Compilers;
using ArduBoy.Compiler.Optimisers;
using ArduBoy.Compiler.Parsers;
using CommandLine;
using CommandLine.Text;

namespace ArduBoy.Compiler.CLI
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var parser = new Parser(with => with.HelpWriter = null);
			var parserResult = parser.ParseArguments<Options>(args);
			parserResult.WithNotParsed(errs => DisplayHelp(parserResult, errs));
			parserResult.WithParsed(Run);
		}

		public static void Run(Options opts)
		{
			WriteColor("Checking paths...", ConsoleColor.Blue);
			opts.ScriptPath = RootPath(opts.ScriptPath);
			opts.OutPath = RootPath(opts.OutPath);
			if (!File.Exists(opts.ScriptPath))
				throw new FileNotFoundException($"Could not find the script file: {opts.ScriptPath}");
			if (!Directory.Exists(opts.OutPath))
				Directory.CreateDirectory(opts.OutPath);
			WriteLineColor("Done!", ConsoleColor.Green);

			var text = File.ReadAllText(opts.ScriptPath);

			WriteColor("Generating AST...", ConsoleColor.Blue);
			IASTGenerator astGenerator = new ArduBoyScriptASTGenerator();
			var ast = astGenerator.Generate(text);
			WriteLineColor("Done!", ConsoleColor.Green);

			WriteColor("Parsing AST...", ConsoleColor.Blue);
			IParser parser = new ArduBoyScriptParser();
			var parsed = parser.Parse(ast);
			WriteLineColor("Done!", ConsoleColor.Green);

			WriteLineColor("Compiling...", ConsoleColor.Blue);
			ICompiler compiler = new ArduBoyScriptCompiler();
			compiler.DoLog += (t) => WriteLineColor($"\t{t}", ConsoleColor.DarkGray);
			var compiled = compiler.Compile(parsed);
			WriteLineColor("Compilation complete!", ConsoleColor.Green);

			WriteLineColor("Optimising...", ConsoleColor.Blue);
			IOptimiser optimiser = new ArduBoyScriptOptimiser();
			optimiser.DoLog += (t) => WriteLineColor($"\t{t}", ConsoleColor.DarkGray);
			var optimised = optimiser.Optimise(parsed);
			WriteLineColor("Optimisation complete!", ConsoleColor.Green);

			WriteColor("Outputting binary...", ConsoleColor.Blue);
			ICodeGenerator codeGenerator = new ArduBoyCodeGenerator();
			var code = codeGenerator.Generate(optimised);
			var targetFile = Path.Combine(opts.OutPath, "game.rbs");
			if (File.Exists(targetFile))
				File.Delete(targetFile);
			File.WriteAllText(targetFile, code);
			WriteLineColor("Done!", ConsoleColor.Green);

			if (opts.ConsoleOut)
				WriteLineColor(code, ConsoleColor.DarkGray);
		}

		private static void HandleParseError(IEnumerable<Error> errs)
		{
			var sentenceBuilder = SentenceBuilder.Create();
			foreach (var error in errs)
				if (error is not HelpRequestedError)
					Console.WriteLine(sentenceBuilder.FormatError(error));
		}

		private static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
		{
			var helpText = HelpText.AutoBuild(result, h =>
			{
				h.AddEnumValuesToHelpText = true;
				return h;
			}, e => e, verbsIndex: true);
			Console.WriteLine(helpText);
			HandleParseError(errs);
		}

		private static string RootPath(string path)
		{
			if (!Path.IsPathRooted(path))
				path = Path.Join(Directory.GetCurrentDirectory(), path);
			path = path.Replace("\\", "/");
			return path;
		}

		private static void WriteLineColor(string text, ConsoleColor? color = null)
		{
			if (color != null)
				Console.ForegroundColor = (ConsoleColor)color;
			else
				Console.ResetColor();
			Console.WriteLine(text);
			Console.ResetColor();
		}

		private static void WriteColor(string text, ConsoleColor? color = null)
		{
			if (color != null)
				Console.ForegroundColor = (ConsoleColor)color;
			else
				Console.ResetColor();
			Console.Write(text);
			Console.ResetColor();
		}
	}
}