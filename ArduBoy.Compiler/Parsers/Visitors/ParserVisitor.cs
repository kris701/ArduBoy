using ArduBoy.Compiler.Models.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Parsers.Visitors
{
    public partial class ParserVisitor
    {
        internal bool IsOfValidNodeType(string content, string nodeType)
        {
            if (content.ToLower().StartsWith(nodeType.ToLower()))
            {
                if (nodeType.Length == content.Length)
                    return true;
                var nextCharacter = content[nodeType.Length];
                if (nextCharacter == ' ')
                    return true;
                if (nextCharacter == '{')
                    return true;
                if (nextCharacter == '\n')
                    return true;
            }
            return false;
        }

        internal bool DoesContentContainNLooseChildren(ASTNode node, string nodeName, int target)
        {
            var looseChildren = ReduceToSingleSpace(RemoveNodeTypeAndEscapeChars(node.Content.ToLower(), nodeName.ToLower()));
            var split = looseChildren.Split(' ');
            var actualCount = split.Length;
            if (split.Length == 1)
                if (split[0] == "")
                    actualCount--;
            if (actualCount != target)
                throw new Exception($"'{nodeName}' is malformed! Expected {target} loose children but got {actualCount}.");
            return true;
        }

        internal string RemoveNodeTypeAndEscapeChars(string content, string nodeType)
        {
            return PurgeEscapeChars(content).Remove(content.IndexOf(nodeType), nodeType.Length).Trim();
        }

        internal string RemoveNodeType(string content, string nodeType)
        {
            return content.Remove(content.IndexOf(nodeType), nodeType.Length).Trim();
        }

        internal string ReduceToSingleSpace(string text)
        {
            while (text.Contains("  "))
                text = text.Replace("  ", " ");
            return text;
        }

        internal string PurgeEscapeChars(string str) => str.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");
    }
}
