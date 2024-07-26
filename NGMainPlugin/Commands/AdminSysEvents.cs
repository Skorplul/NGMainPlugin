using CommandSystem;
using System;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using EvParams = NGMainPlugin.Systems.SystemEvents.Events;


namespace NGMainPlugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class AdminSysEvents : ParentCommand
    {
        public AdminSysEvents() => LoadGeneratedCommands();

        public override string Command { get; } = "test";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Test something :^)";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if(player == null)
            {
                response = "Player is null, please contact a Dev!";
                return false;
            }
            if (!player.CheckPermission("NG.Events"))
            {
                response = "You don't have the permission for that!";
                return false;
            }
            if (Round.IsStarted)
            {
                response = "You need to use this in the Lobby!";
                return false;
            }
            if (EvParams.EventRound)
            {
                response = "An event is already running!";
                return false;
            }


            EventRound = true;

            response = "Event has been triggered.";
            return true;
        }
    }
}