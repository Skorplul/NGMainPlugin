﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp079;
using Exiled.Events.EventArgs.Player;
using PluginAPI.Events;

namespace NGMainPlugin
{
    public class EventHandlers
    {
        private readonly Main plugin;
        public static int PcCurentLvl;

        public void OnRoundStarted()
        {
            PcCurentLvl = 1;
            Commands.SCPSwap.swaped.Clear();
            Commands.Durchsage.spoke.Clear();
        }

        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (plugin.Config.NoTesTuts) 
                ev.IsAllowed = false;
        }

        public void OnSCP079GainingLvl(GainingLevelEventArgs ev)
        {
            PcCurentLvl = ev.NewLevel;
        }
    }
}
