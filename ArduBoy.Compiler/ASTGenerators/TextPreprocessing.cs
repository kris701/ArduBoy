using ArduBoy.Compiler.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.ASTGenerators
{
    public static class TextPreprocessing
    {
        public static string ReplaceSpecialCharacters(string text)
        {
            text = text.Replace('\r', ' ');
            text = text.Replace('\t', ' ');
            return text;
        }

        public static string ReplaceCommentsWithWhiteSpace(string text)
        {
            if (!text.EndsWith(ASTTokens.BreakToken))
                text += ASTTokens.BreakToken;

            var retStr = text;
            int offset = 0;
            while (retStr.Contains("//"))
            {
                int from = retStr.IndexOf("//", offset);
                int to = retStr.IndexOf(ASTTokens.BreakToken, from);
                retStr = StringHelpers.ReplaceRangeWithSpacesFast(retStr, from, to);
                offset = to + 1;
            }
            return retStr;
        }
    }
}
