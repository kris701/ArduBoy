namespace ArduBoy.Compiler.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var compiler = new ArduBoyCompiler();
            Console.WriteLine(compiler.Compile(File.ReadAllText("test.txt")));
        }
    }
}