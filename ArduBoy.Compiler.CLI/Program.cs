using System;

namespace ArduBoy.Compiler.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var compiler = new ArduBoyCompiler();
            var text = 
                "{DEFINE test 23}" + Environment.NewLine +
                "{:label}" + Environment.NewLine +
                "{IF 1 == test {GOTO end}}" + Environment.NewLine +
                "{GOTO label}" + Environment.NewLine +
                "{:end}";

            Console.WriteLine(compiler.Compile(text));
        }
    }
}