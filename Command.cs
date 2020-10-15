namespace LouveSystems.CommandLineInterface
{
    public abstract class Command
    {
        public abstract string HelpMessage { get; }
        public abstract string HelpfulArguments { get; }
        public abstract bool Execute(CommandLineInterface cli, string arguments, out string remainder);
    }
}
