using System;

namespace LouveSystems.CommandLineInterface.DefaultCommands
{
    class CommandSeparator : Command
    {
        public override string HelpMessage => "Gets or Sets the character used to tell two consecutive commands apart";
        public override string HelpfulArguments => "<single-character-separator>";

        public override bool Execute(CommandLineInterface cli, string arguments, out string remainder)
        {
            if (arguments.Length == 0)
            {
                remainder = string.Empty;
                cli.WriteLine($"The current command separator is the character: [{cli.CommandSeparator}]");
            }
            else
            {
                cli.CommandSeparator = cli.GetFirstString(arguments, out remainder)[0];
                cli.WriteLine($"The command separator is now the character: [{cli.CommandSeparator}]");
            }

            return true;
        }
    }
}
