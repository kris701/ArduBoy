using System;

namespace ArduBoy.Compiler.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var compiler = new ArduBoyCompiler();
            var text = 
                "{DEFINE true 1}" + Environment.NewLine +
                "{:label}" + Environment.NewLine +
                "{IF INPUT(UP) == true " + Environment.NewLine +
                "   {GOTO end}" + Environment.NewLine +
                "   {GOTO end}}" + Environment.NewLine +
                "{WAIT 1000}" + Environment.NewLine +
                "{GOTO label}" + Environment.NewLine +
                "{:end}";

            Console.WriteLine(compiler.Compile(text));
        }
    }
}