using CommandSystem;
using System;
using Exiled.API.Features;
using PlayerRoles;
using System.Linq;
using System.Collections.Generic;

namespace NGMainPlugin.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Nick : ParentCommand
    {
        public static List<string> spoke = new List<string>();

        public static Main Plugin { get; set; }

        public Nick()
        {
            LoadGeneratedCommands();
        }

        public override string Command { get; } = "nick";

        public override string[] Aliases { get; } = new string[] {  };

        public override string Description { get; } = "Change your nickname.";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (arguments.Count() < 1)
            {
                response = "Usage: .durchsage <Your Message Here>";
                return false;
            }
            if (player == null)
            {
                response = "Player is null, contact an admin/developer!";
                return false;
            }
            if (arguments.Array[1] == "remove")
            {
                player.DisplayNickname = null;

                response = "Your custom nick has been removed!";
                return true;
            }

            string NewName = string.Join("_", arguments.Skip(0));
            player.DisplayNickname = NewName;

            response = "Your new nick has been set!";
            return true;
        }
    }
}