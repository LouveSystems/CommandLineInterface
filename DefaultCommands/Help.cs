using System;
using System.Linq;

namespace LouveSystems.CommandLineInterface.DefaultCommands
{
    class Help : Command
    {
        public override string HelpMessage => "Gives a short description of each command";
        public override string HelpfulArguments => string.Empty;

        public override bool Execute(CommandLineInterface cli, string arguments, out string remainder)
        {
            var commands = cli.CommandList.OrderBy(o=>o.name);
            foreach (var command in commands)
            {
                cli.Color(ConsoleColor.Cyan);
                cli.Write($"    {command.name}");
                cli.Color(ConsoleColor.DarkGray);
                cli.Write($"{(command.command.HelpfulArguments == string.Empty ? string.Empty : $" {command.command.HelpfulArguments}")}");
                cli.Write(" => ");
                cli.Color(ConsoleColor.White);
                cli.Write(command.command.HelpMessage);
                cli.WriteLine();
            }
            remainder = arguments;
            return true;
        }
    }
}
