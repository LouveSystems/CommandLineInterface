using System;

namespace LouveSystems.CommandLineInterface.DefaultCommands
{
    class ContextSeparator : Command
    {
        public override string HelpMessage => "Gets or Sets the character used to separate a command and their arguments";
        public override string HelpfulArguments => "<single-character-separator>";

        public override bool Execute(CommandLineInterface cli, string arguments, out string remainder)
        {
            if (arguments.Length == 0)
            {
                remainder = string.Empty;
                cli.WriteLine($"The current context separator is the character: [{cli.ContextSeparator}]");
            }
            else
            {
                cli.ContextSeparator = cli.GetFirstString(arguments, out remainder)[0];
                cli.WriteLine($"The context separator is now the character: [{cli.ContextSeparator}]");
            }

            return true;
        }
    }
}
