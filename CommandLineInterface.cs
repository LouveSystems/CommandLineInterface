using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LouveSystems.CommandLineInterface
{
    public class CommandLineInterface
    {
        public const float VERSION = 1.00f;
        const string welcomeMessage =
@"Welcome to the {0} command line interface! Type {1} to get a list of commands.
Press ENTER to submit a command.
rackover@louve.systems (2020)";
        const string titleFormat =
@"{0} (LouveSystems' Command Line Interface v{1})";

        public char StringEnclosure { private set; get; } = '"';
        public char ArgumentSeparator { set; get; } = ' ';
        public char CommandSeparator { set; get; } = ' ';
        public char ContextSeparator { set; get; } = ' ';

        public Dictionary<string, Command> Commands
        {
            get
            {
                return commands.ToDictionary(o => o.Key, o => o.Value);
            }
        }

        public IReadOnlyList<(string name, Command command)> CommandList
        {
            get
            {
                return commands.Select(o => (o.Key, o.Value)).ToList().AsReadOnly();
            }
        }

        readonly Dictionary<string, Command> defaultCommands = new Dictionary<string, Command>()
        {
            {"HELP",  new DefaultCommands.Help()},
            {"ARGSEP",  new DefaultCommands.ArgumentSeparator()},
            {"COMSEP",  new DefaultCommands.CommandSeparator()},
            {"CTXSEP",  new DefaultCommands.ContextSeparator()},
            {"VERSION",  new DefaultCommands.Version()},
            {"EXIT",  new DefaultCommands.Exit()}
        };

        readonly string helpKeyword = "HELP";

        string name;

        Dictionary<string, Command> commands = new Dictionary<string, Command>();

        public CommandLineInterface(string name, List<(string name, Command command)> commands = null, bool clearDefaultCommands = false)
        {
            SetName(name);
            this.commands = clearDefaultCommands ? new Dictionary<string, Command>() : new Dictionary<string, Command>(defaultCommands);

            if (commands != null)
            {
                foreach (var command in commands)
                {
                    SetCommand(command.name, command.command);
                }
            }
        }

        public string GetFirstString(string argumentString, out string remainder)
        {
            StringBuilder argumentBuilder = new StringBuilder();
            bool inString = false;
            int limitIndex;
            for (limitIndex = 0; limitIndex < argumentString.Length; limitIndex++)
            {
                char currentCharacter = argumentString[limitIndex];
                if (currentCharacter == ArgumentSeparator)
                {
                    if (!inString)
                    {
                        break;
                    }
                }
                else if (currentCharacter == StringEnclosure)
                {
                    if (inString)
                    {
                        inString = false;
                        break;
                    }
                    else
                    {
                        inString = true;
                        continue;
                    }
                }

                argumentBuilder.Append(currentCharacter);
            }

            string firstString = argumentBuilder.ToString();
            remainder = limitIndex == argumentString.Length ? string.Empty : argumentString.Substring(limitIndex + 1);

            return firstString;
        }

        public void Err(string message)
        {
            Color(ConsoleColor.Red);
            WriteLine($"ERROR: {message}");
            Color(ConsoleColor.Gray);
        }

        public void Warn(string message)
        {
            Color(ConsoleColor.Yellow);
            WriteLine($"WARNING: {message}");
            Color(ConsoleColor.Gray);
        }

        public void Log(string message = "")
        {
            WriteLine(message);
        }

        public virtual void Write(string str) => Console.Write(str);

        public virtual void WriteLine(string str) => Console.WriteLine(str);

        public virtual void WriteLine() => Console.WriteLine();

        public virtual void Color(ConsoleColor color) => Console.ForegroundColor = color;

        public virtual string ReadInput() => Console.ReadLine();

        public virtual void SetTitle(string title) => Console.Title = string.Format(titleFormat, name, VERSION);

        public void SetName(string newName)
        {
            name = newName;
            SetTitle(name);
        }

        protected void SetCommand(string keyword, Command command)
        {
            commands[keyword] = command;
        }

        protected void SetDefaultCommand(string keyword)
        {
            if (defaultCommands.TryGetValue(keyword, out var command))
            {
                SetCommand(keyword, command);
            }
        }

        protected void RemoveCommand(string keyword)
        {
            if (defaultCommands.ContainsKey(keyword))
            {
                commands.Remove(keyword);
            }
        }

        protected void Run(string initialCommand = null)
        {
            Log(string.Format(welcomeMessage, name, helpKeyword));

            // Block thread
            while (true)
            {
                Write("> ");
                string typedCommand = initialCommand ?? ReadInput();
                if (initialCommand == null) Log();
                initialCommand = null;
                typedCommand.Replace(Environment.NewLine, " ");

                while (!string.IsNullOrWhiteSpace(typedCommand))
                {
                    var spacePosition = typedCommand.IndexOf(ContextSeparator);
                    string arguments = string.Empty;
                    if (spacePosition > 0)
                    {
                        arguments = typedCommand.Substring(spacePosition + 1);
                        typedCommand = typedCommand.Substring(0, spacePosition);
                    }

                    while (typedCommand[0] == ' ')
                    {
                        typedCommand = typedCommand.Substring(1);
                    }

                    if (commands.TryGetValue(typedCommand.ToUpper(), out Command command))
                    {
#if DEBUG
                        try
                        {
#endif
                            if (!command.Execute(this, arguments, out typedCommand))
                            {
                                typedCommand = string.Empty;
                            }
#if DEBUG
                        }
                        catch (Exception e)
                        {
                            Err(e.ToString());
                            typedCommand = string.Empty;
                        }
#endif

                        Log();
                    }
                    else
                    {
                        Err($"Command not found: {typedCommand.ToUpper()}. Type {helpKeyword} to get a list of commands.");
                        typedCommand = string.Empty;
                        Log();
                    }
                }
            }
        }
    }
}
