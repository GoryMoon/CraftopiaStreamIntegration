using System;

namespace CraftopiaStreamIntegration.Commands
{
    public interface ICommand
    {
        string Name { get; }

        string Desc { get; }
        
        bool Handle(string sender, string[] args, Action<string> sendMessage);
    }
}