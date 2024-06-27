using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script
{
    public interface INode
    {
        public List<T> FindTypes<T>(List<Type>? stopIf = null, bool ignoreFirst = false);
        public void FindTypes<T>(List<T> returnSet, List<Type>? stopIf = null, bool ignoreFirst = false);
    }
}
