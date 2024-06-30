﻿namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class GotoExp : BaseNode, IExp
    {
        public string Target { get; set; }

        public GotoExp(string target)
        {
            Target = target;
        }

        public override string ToString() => $"_goto {Target}";
    }
}