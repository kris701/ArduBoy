using ArduBoy.Compiler.CodeGenerators.Visitors;
using ArduBoy.Compiler.Models.Script;

namespace ArduBoy.Compiler.CodeGenerators
{
    public class ArduBoyCodeGenerator : ICodeGenerator
    {
        public string Generate(ArduBoyScriptDefinition script)
        {
            var visitor = new GeneratorVisitors();
            var retStr = visitor.Visit((dynamic)script);
            while (retStr.Contains($"{Environment.NewLine}{Environment.NewLine}"))
                retStr = retStr.Replace($"{Environment.NewLine}{Environment.NewLine}", Environment.NewLine);
            retStr = retStr.Replace(Environment.NewLine, "\n");

			retStr = ConvertLineIndexesToCharacterIndexes(retStr);

            return retStr;
        }

        private string ConvertLineIndexesToCharacterIndexes(string text)
        {
			var targets = new Dictionary<int, int>();
            var lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
				if (lines[i].StartsWith($"{OperatorCodes.GetByteCode(":call")} "))
                {
                    var target = lines[i].Split(' ')[1];
                    for (int j = 0; j < lines.Length; j++)
                    {
                        if (lines[j].StartsWith($"{OperatorCodes.GetByteCode(":")} {target}"))
                        {
                            targets.Add(i, j);
                            break;
                        }
                    }
                }
                else if (lines[i].StartsWith($"{OperatorCodes.GetByteCode(":goto")} "))
                {
                    var target = lines[i].Split(' ')[1];
                    for (int j = 0; j < lines.Length; j++)
                    {
                        if (lines[j].StartsWith($"{OperatorCodes.GetByteCode(":")} {target}"))
                        {
                            targets.Add(i, j);
                            break;
                        }
                    }
                }
                else if (lines[i].StartsWith($"{OperatorCodes.GetByteCode(":if")} "))
                {
                    var target = int.Parse(lines[i].Split(' ')[1]);
                    targets.Add(i, i + target);
                }
            }
            for (int i = 0; i < lines.Length; i++)
                if (lines[i].StartsWith($"{OperatorCodes.GetByteCode(":")} "))
                    lines[i] = "";

            bool changed = true;
            while (changed)
            {
                changed = false;
                foreach (var key in targets.Keys)
                {
                    var line = lines[key];
                    if (line.StartsWith($"{OperatorCodes.GetByteCode(":call")} "))
                    {
                        var newTarget = GetCharacterByLine(lines, targets[key]);
                        if (lines[key] != $"{OperatorCodes.GetByteCode(":call")} {newTarget}")
                        {
                            changed = true;
                            lines[key] = $"{OperatorCodes.GetByteCode(":call")} {newTarget}";
                        }
                    }
                    else if (line.StartsWith($"{OperatorCodes.GetByteCode(":goto")} "))
                    {
                        var newTarget = GetCharacterByLine(lines, targets[key]);
                        if (lines[key] != $"{OperatorCodes.GetByteCode(":goto")} {newTarget}")
                        {
                            changed = true;
                            lines[key] = $"{OperatorCodes.GetByteCode(":goto")} {newTarget}";
                        }
                    }
                    else if (line.StartsWith($"{OperatorCodes.GetByteCode(":if")} "))
                    {
                        var newTarget = GetCharacterByLine(lines, targets[key]);
                        if (!lines[key].StartsWith($"{OperatorCodes.GetByteCode(":if")} {newTarget}"))
                        {
                            changed = true;
                            var split = lines[key].Split(' ');
                            split[1] = $"{newTarget}";
                            lines[key] = string.Join(' ', split);
                        }
                    }
                }
            }

            return string.Join('\n', lines);
        }

        private int GetCharacterByLine(string[] lines, int line)
        {
            var count = 1;
            for (int i = 0; i < lines.Length; i++)
            {
                count += lines[i].Length + 1;
                if (i >= line)
                    break;
            }
            return count;
        }
    }
}
