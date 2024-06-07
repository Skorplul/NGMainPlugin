using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp079;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using MEC;
using NGMainPlugin.Systems.Liftaudio;

namespace NGMainPlugin
{
    public class EventHandlers
    {
        private readonly Main plugin;

        public static int PcCurentLvl;
        readonly Random random = new Random();

        public EventHandlers (Main plugin)
        {
            this.plugin = plugin;
        }

        public void OnRoundStarted()
        {
            PcCurentLvl = 1;
            Commands.SCPSwap.swaped.Clear();
            Commands.Durchsage.spoke.Clear();

            foreach (Lift lift in (IEnumerable<Lift>)Lift.List)
                Timing.RunCoroutine(Methods.CheckingPlayerLift(lift));
        }

        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (ev.Player.Role == RoleTypeId.Tutorial)
            {
                if (plugin.Config.NoTesTuts)
                {
                    ev.IsAllowed = false;
                }
            }
        }

        public void OnSCP079GainingLvl(GainingLevelEventArgs ev)
        {
            PcCurentLvl = ev.NewLevel;
        }

        public void OnBan(BannedEventArgs ev)
        {
            Map.Broadcast(6, $"[< color =#f67979>N</color><color=#e86e6c>e</color><color=#d96260>x</color><color=#cb5754>u</color><color=#bd4c48>s</color><color=#af413c>G</color><color=#a13631>a</color><color=#932a26>m</color><color=#851f1b>i</color><color=#771211>n</color><color=#6a0303>g</color>]: {ev.Player.Nickname} has been banned from the server!");
        }

        public void OnKick(KickedEventArgs ev)
        {
            Map.Broadcast(6, $"[< color =#f67979>N</color><color=#e86e6c>e</color><color=#d96260>x</color><color=#cb5754>u</color><color=#bd4c48>s</color><color=#af413c>G</color><color=#a13631>a</color><color=#932a26>m</color><color=#851f1b>i</color><color=#771211>n</color><color=#6a0303>g</color>]: {ev.Player.Nickname} has been kicked from the server!");
        }

    }
}
