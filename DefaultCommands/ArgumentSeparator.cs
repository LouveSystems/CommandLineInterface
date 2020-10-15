using System;

namespace LouveSystems.CommandLineInterface.DefaultCommands
{
    class ArgumentSeparator : Command
    {
        public override string HelpMessage => "Gets or Sets the character used to tell two consecutive arguments apart";
        public override string HelpfulArguments => "<single-character-separator>";

        public override bool Execute(CommandLineInterface cli, string arguments, out string remainder)
        {
            if (arguments.Length == 0)
            {
                remainder = string.Empty;
                cli.WriteLine($"The current argument separator is the character: [{cli.ArgumentSeparator}]");
            }
            else
            {
                cli.ArgumentSeparator = cli.GetFirstString(arguments, out remainder)[0];
                cli.WriteLine($"The argument separator is now the character: [{cli.ArgumentSeparator}]");
            }

            return true;
        }
    }
}
