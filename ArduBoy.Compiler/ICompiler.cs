using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler
{
    public interface ICompiler
    {
        public string Compile(string from);
    }
}
