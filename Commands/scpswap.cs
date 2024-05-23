﻿using CommandSystem;
using System;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using PlayerRoles;
using System.Collections.Generic;

namespace NGMainPlugin.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class SCPSwap : ParentCommand
    {
        public static List<string> swaped = new List<string>();

        public SCPSwap() => LoadGeneratedCommands();

        public override string Command { get; } = "scpswap";

        public override string[] Aliases { get; } = new string[] { "scps" };

        public override string Description { get; } = "Change your scp!";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (arguments.Count < 1)
            {
                response = "Usage: .scpswap {scpNumber}";
                return false;
            }
            if (player == null)
            {
                response = "ERROR: The player is null, please contact an admin/developer!";
                return false;
            }
            if (!player.IsScp)
            {
                response = "You can only use this as SCP.";
                return false;
            }
            if (Main.SwapTimeout == null)
            {
                response = "Timeout time hasn't been set properly, please contact an admin/developer!";
                return false;
            }
            if (Round.ElapsedTime > Main.SwapTimeout)
            {
                response = "You can no longer swap, sorry but you're too late.";
                return false;
            }
            if (arguments.Array[1] == "0492" || arguments.Array[1] == "049-2")
            {
                response = "You can't swap to a Zombi";
                return false;
            }
            RoleTypeId GetSwap(string scpNum)
            {
                switch (scpNum)
                {
                    case "049":
                        return RoleTypeId.Scp049;
                    case "079":
                        return RoleTypeId.Scp079;
                    case "096":
                        return RoleTypeId.Scp096;
                    case "106":
                        return RoleTypeId.Scp106;
                    case "939":
                        return RoleTypeId.Scp939;
                    case "173":
                        return RoleTypeId.Scp173;
                    default:
                        return RoleTypeId.None;
                }
            }
            if (GetSwap(arguments.Array[1]) == RoleTypeId.None)
            {
                response = "Not a valid SCP!";
                return false;
            }
            if (player.Role == GetSwap(arguments.Array[1]))
            {
                response = "You can't swap to the same SCP you already are!";
                return false;
            }
            foreach (Player ply in Player.List)
            {
                if (ply.IsScp)
                {
                    if (ply.Role == GetSwap(arguments.Array[1]))
                    {
                        response = "Someone is this SCP already!";
                        return false;
                    }
                }
            }
            if (Main.OneTimeSwap)
            {
                if (swaped.Contains(player.UserId))
                {
                    response = "You already swaped in this round!";
                    return false;
                }
            }

            swaped.Add(player.UserId);

            player.Role.Set(GetSwap(arguments.Array[1]));
            response = $"You are now SCP{arguments.Array[1]}!";
            return true;
        }
    }
}
