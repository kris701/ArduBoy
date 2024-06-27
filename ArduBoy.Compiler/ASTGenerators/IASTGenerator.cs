using ArduBoy.Compiler.Models.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.ASTGenerators
{
    public interface IASTGenerator
    {
        public ASTNode Generate(string text);
    }
}
