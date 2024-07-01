﻿namespace ArduBoy.Compiler.CodeGenerators
{
    public static class OperatorCodes
    {
        private static readonly Dictionary<string, byte> _opCodes = new Dictionary<string, byte>()
        {
            { ":", 0x0 },
            { "-", 0x0 },
            { ":call", 0x1 },
            { ":if", 0x2 },
            { ":wait", 0x3 },
            { ":set", 0x4 },
            { ":audio", 0x5 },
            { ":draw-line", 0x6 },
            { ":goto", 0x7 },
            { ":add", 0x8 },
            { ":sub", 0x9 },
            { ":mult", 0x10 },
            { ":div", 0x11 },

            { ":draw-circle", 0x12 },
            { ":draw-fill-circle", 0x13 },
            { ":draw-triangle", 0x14 },
            { ":draw-fill-triangle", 0x15 },
            { ":draw-rect", 0x16 },
            { ":draw-fill-rect", 0x17 },
            { ":draw-text", 0x18 },
            { ":draw-fill", 0x19 },

            { "==", 0x90 },
            { "<", 0x91 },
            { ">", 0x92 },
            { "!=", 0x93 },
        };

        public static string GetByteCode(string id)
        {
#if DEBUG
            return id;
#endif
            return $"{_opCodes[id.ToLower()]}";
        }
    }
}
