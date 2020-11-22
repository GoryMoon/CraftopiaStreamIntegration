using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CraftopiaStreamIntegration.Commands;

namespace CraftopiaStreamIntegration
{
    public class CommandManager
    {
        public static readonly CommandManager Instance = new CommandManager();
        private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

        private CommandManager()
        {
            RegisterCommand(new HelpCommand());
        }

        public void RegisterCommand(ICommand command)
        {
            if (!_commands.ContainsKey(command.Name))
            {
                _commands.Add(command.Name, command);
            }
        }

        public void HandleMessage(string sender, string message)
        {
            var strings = message.Substring(1).Split(' ');
            if (strings.Length > 0 && _commands.ContainsKey(strings[0]))
            {
                var command = _commands[strings[0]];
                try
                {
                    strings = strings.Length > 1 ? new List<string>(strings).GetRange(1, strings.Length).ToArray() : new string[0];
                    command.Handle(sender, strings, SendMessage);
                }
                catch (Exception e)
                {
                    CSIPlugin.Instance.Log.LogError(e);
                    SendMessage($"<color=\"red\">Error executing command: {command.Name}");
                }
                return;
            }
            
            SendMessage("<color=\"red\">Invalid command, use /help to get a list of commands");
        }

        private static void SendMessage(string message)
        {
            AccessUtils.PopMessage(5, "Integration", message);
        }

        private class HelpCommand: ICommand
        {
            public string Name => "help";
            public string Desc => "Prints this list of commands";
            public bool Handle(string sender, string[] args, Action<string> sendMessage)
            {
                var builder = new StringBuilder();
                builder.Append("Commands:\n");
                var maxLength = Instance._commands.Keys.Select(s => s.Length).Max();
                
                foreach (var pair in Instance._commands)
                {
                    builder.Append($" /{pair.Key}".PadRight(maxLength + 6) + $"- {pair.Value.Desc}\n");
                }
                SendMessage(builder.ToString());

                return true;
            }
        }
    }
}