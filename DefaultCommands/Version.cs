using System;

namespace LouveSystems.CommandLineInterface.DefaultCommands
{
    class Version : Command
    {
        public override string HelpMessage => "Prints the CLI version";
        public override string HelpfulArguments => string.Empty;

        public override bool Execute(CommandLineInterface cli, string arguments, out string remainder)
        {
            cli.WriteLine($"LouveSystems' Command Line Interface version {CommandLineInterface.VERSION.ToString("n2")}");
            remainder = arguments;
            return true;
        }
    }
}
