using ArduBoy.Compiler.Models.AST;
using System.Text;

namespace ArduBoy.Compiler.ASTGenerators
{
    public class ArduBoyScriptASTGenerator : IASTGenerator
    {
        public ASTNode Generate(string text)
        {
            text = text.Trim();
            var node = GenerateRec(text);
            return node;
        }

        private ASTNode GenerateRec(string text)
        {
            if (text.Contains('('))
            {
                var firstP = text.IndexOf('(');
                var lastP = text.LastIndexOf(')');
                if (lastP == -1)
                    throw new Exception($"Node started with a '(' but didnt end with one!: {text}");
                var excludeSlices = new List<int>();

                var children = new List<ASTNode>();
                int offset = firstP;
                while (text.IndexOf('(', offset + 1) != -1)
                {
                    int currentLevel = 0;
                    int startP = text.IndexOf('(', offset + 1);
                    int endP = text.Length;
                    for (int i = startP + 1; i < text.Length; i++)
                    {
                        if (text[i] == '(')
                            currentLevel++;
                        else if (text[i] == ')')
                        {
                            if (currentLevel == 0)
                            {
                                endP = i + 1;
                                break;
                            }
                            currentLevel--;
                        }
                    }

                    offset = endP - 1;
                    excludeSlices.Add(startP);
                    excludeSlices.Add(endP);
                    var newContent = text.Substring(startP, endP - startP);
                    children.Add(GenerateRec(newContent));
                }
                var newInnerContent = GenerateInnerContent(text, excludeSlices);
                firstP = newInnerContent.IndexOf('(');
                lastP = newInnerContent.LastIndexOf(')');
                newInnerContent = newInnerContent.Substring(firstP + 1, lastP - firstP - 1);

                return new ASTNode(newInnerContent.Trim(), children);
            }
            else
                return new ASTNode(text.Trim());
        }

        private string GenerateInnerContent(string innerContent, List<int> slices)
        {
            if (slices.Count == 0)
                return innerContent;
            var sb = new StringBuilder();

            slices = slices.Order().ToList();

            var from = 0;
            for (int i = 0; i < slices.Count; i += 2)
            {
                sb.Append(innerContent.Substring(from, slices[i] - from));
                from = slices[i + 1];
            }
            sb.Append(innerContent.Substring(from));

            return sb.ToString();
        }
    }
}
