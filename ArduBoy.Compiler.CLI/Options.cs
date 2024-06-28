using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.CLI
{
    public class Options
    {
        [Option('s', "script", Required = true, HelpText = "Path to the script file to use.")]
        public string ScriptPath { get; set; } = "";
        [Option('o', "out", Required = false, HelpText = "Path to output to.")]
        public string OutPath { get; set; } = "out";

        [Option('c', "console", Required = false, HelpText = "If the resulting binary should be printed to STDout.")]
        public bool ConsoleOut { get; set; } = false;
    }
}
