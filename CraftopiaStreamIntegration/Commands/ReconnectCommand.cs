using System;

namespace CraftopiaStreamIntegration.Commands
{
    public class ReconnectCommand: ICommand
    {
        public string Name => "reconnect";
        public string Desc => "Reconnects to the integration app";
        public bool Handle(string sender, string[] args, Action<string> sendMessage)
        {
            CSIPlugin.Instance.IntegrationManager.Close();
            CSIPlugin.Instance.IntegrationManager.Start();
            return true;
        }
    }
}