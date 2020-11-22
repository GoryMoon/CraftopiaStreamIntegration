using System;

namespace CraftopiaStreamIntegration.Commands
{
    public class DumpCommand: ICommand
    {
        public string Name => "dump";
        public string Desc => "Dumps the items and enchantments to game folder";
        public bool Handle(string sender, string[] args, Action<string> sendMessage)
        {
            sendMessage.Invoke("<color=\"yellow\">Starting dump...");
            Utils.DumpItems();
            sendMessage.Invoke("<color=\"green\">Dump complete");
            return true;
        }
    }
}