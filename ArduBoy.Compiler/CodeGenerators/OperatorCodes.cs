namespace ArduBoy.Compiler.CodeGenerators
{
    public class OperatorCodes
    {
        public static byte GotoLabelCode = 0x0;
        public static byte GotoCode = 0x1;

        public static byte IfCode = 0x2;

        public static byte EQCode = 0x3;
        public static byte LTCode = 0x4;
        public static byte GTCode = 0x5;
        public static byte NEQCode = 0x6;

        public static byte WaitCode = 0x7;
    }
}
