using ArduBoy.Compiler.Models.AST;
using ArduBoy.Compiler.Models.Exceptions;

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
				if (nextCharacter == '(')
					return true;
				if (nextCharacter == '\n')
					return true;
			}
			return false;
		}

		internal bool DoesNotContainStrayCharacters(ASTNode node, string targetName)
		{
			if (node.Content.Replace(targetName, "").Trim() != "")
				throw new ParserException(node, $"The node '{targetName}' has unknown content inside! Contains stray characters: {node.Content.Replace(targetName, "").Trim()}");
			return true;
		}

		internal bool DoesNodeHaveSpecificChildCount(ASTNode node, string nodeName, int targetChildren)
		{
			if (targetChildren == 0)
			{
				if (node.Children.Count != 0)
				{
					//throw new Exception($"'{nodeName}' must not contain any children!");
					return false;
				}
			}
			else
			{
				if (node.Children.Count != targetChildren)
				{
					//throw new Exception($"'{nodeName}' must have exactly {targetChildren} children, but it has '{node.Children.Count}'!");
					return false;
				}
			}
			return true;
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
			{
				//throw new Exception($"'{nodeName}' is malformed! Expected {target} loose children but got {actualCount}.");
				return false;
			}
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

		internal List<ASTNode> GetEmptyNode(ASTNode from, int offset = 0)
		{
			if (from.Children[offset].Content == "")
				return from.Children[offset].Children;
			return from.Children.Skip(offset).ToList();
		}
	}
}
