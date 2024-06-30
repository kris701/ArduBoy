using ArduBoy.Compiler.CodeGenerators.Visitors;
using ArduBoy.Compiler.Models.Script;
using System.Linq;

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

            retStr = IndexCallLines(retStr);

            return retStr;
        }

        private string IndexCallLines(string text)
        {
            var lines = text.Split(Environment.NewLine);
            for(int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith($"{OperatorCodes.GetByteCode(":call")} "))
                {
                    var target = lines[i].Split(' ')[1];
                    for (int j = 0; j < lines.Length; j++)
                    {
                        if (lines[j].StartsWith($"{OperatorCodes.GetByteCode(":")} {target}"))
                        {
                            lines[i] = $"{OperatorCodes.GetByteCode(":call")} {j}";
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
                            lines[i] = $"{OperatorCodes.GetByteCode(":goto")} {j}";
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < lines.Length; i++)
                if (lines[i].StartsWith($"{OperatorCodes.GetByteCode(":")} "))
                    lines[i] = "";
            return string.Join('\n', lines);
        }
    }
}
