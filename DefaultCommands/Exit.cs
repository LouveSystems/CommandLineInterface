using System;

namespace LouveSystems.CommandLineInterface.DefaultCommands
{
    class Exit : Command
    {
        public override string HelpMessage => "Exits the program gracefully";
        public override string HelpfulArguments => string.Empty;

        public override bool Execute(CommandLineInterface cli, string arguments, out string remainder)
        {
            Environment.Exit(0);
            remainder = string.Empty;
            return true;
        }
    }
}
