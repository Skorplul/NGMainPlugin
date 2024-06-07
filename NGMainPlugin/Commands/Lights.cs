using CommandSystem;
using System;
using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NGMainPlugin.NGMainPlugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Lights : ParentCommand
    {
        public static Main Plugin { get; set; }

        public Lights()
        {
            LoadGeneratedCommands();
        }

        public override string Command { get; } = "lights";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Change the lights of the Facillaty.";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (player == null)
            {
                response = "Player Null! Contact a Developer!";
                return false;
            }
            if (arguments.Count == 0)
            {
                Map.ResetLightsColor();
                response = "Reset all Lights.";
                return true;
            }
            if (arguments.Count < 3)
            {
                response = "Usage: lights (r/g/b)";
                return false;
            }

            try
            {
                byte r = byte.Parse(arguments.Array[1]);
                byte g = byte.Parse(arguments.Array[2]);
                byte b = byte.Parse(arguments.Array[3]);



                UnityEngine.Color NewMapLights = new UnityEngine.Color(r, g, b);
                Map.ChangeLightsColor(NewMapLights);
                response = "Done";
                return true;
            }
            catch (FormatException e)
            {
                response = "One of the color values is not a valid float: " + e.Message;
                return false;
            }
            catch (Exception e)
            {
                response = "An error occurred: " + e.Message;
                return false;
            }
        }
    }
}
