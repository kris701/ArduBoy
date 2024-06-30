namespace ArduBoy.Compiler.CodeGenerators
{
    public static class OperatorCodes
    {
        private static Dictionary<string, byte> _opCodes = new Dictionary<string, byte>()
        {
            { ":", 0x0 },
            { ":call", 0x1 },
            { ":if", 0x2 },
            { ":wait", 0x3 },
            { ":set", 0x4 },
            { ":audio", 0x5 },
            { ":draw-line", 0x6 },
            { ":goto", 0x7 },

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
