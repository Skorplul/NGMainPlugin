using CommandSystem;
using RemoteAdmin;
using System;
using Exiled;
using System.Reflection;
using System.Text;
using Steamworks.Data;
using Exiled.API.Features;
using System.Linq;
using Exiled.Permissions.Extensions;
using System.Collections.Specialized;
using System.Net;
using LiteDB;


namespace NGMainPlugin.Commands.Test
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Test : ParentCommand
    {
        public Test() => LoadGeneratedCommands();

        public override string Command { get; } = "test";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Test something :^)";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            
            

            response = "No permission";
            return false;
        }
    }
}