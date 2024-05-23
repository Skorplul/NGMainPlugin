using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using Server = Exiled.Events.Handlers.Server;
using Player = Exiled.Events.Handlers.Player;
using Map = Exiled.Events.Handlers.Map;
using SCP079 = Exiled.Events.Handlers.Scp079;
using CommandSystem;
using NGMainPlugin.Commands;

namespace NGMainPlugin
{
    public class Main : Plugin<Config>
    {
        public override string Author { get; } = "Skorp 1.0, mtf_alpha_one, Dashtiss";
        public override string Name { get; } = "NGMainPlugin";
        public override string Prefix { get; } = "NGM";
        public override Version Version { get; } = new Version(0, 1, 0);

        public override Version RequiredExiledVersion { get; } = new Version(8, 5, 0);

        public EventHandlers EventHandlers;
        public static Main Instance { get; private set; }

        public override void OnEnabled()
        {
            base.OnEnabled();

            Instance = this;
            EventHandlers = new EventHandlers(this);
            Player.TriggeringTesla += EventHandlers.OnTriggeringTesla;
            SCP079.GainingLevel += EventHandlers.OnSCP079GainingLvl;
            Server.RoundStarted += EventHandlers.OnRoundStarted;

            SCPSwap.Plugin = this;
            Durchsage.Plugin = this;

        }

        public override void OnDisabled()
        {
            Player.TriggeringTesla -= EventHandlers.OnTriggeringTesla;
            SCP079.GainingLevel -= EventHandlers.OnSCP079GainingLvl;
            Server.RoundStarted -= EventHandlers.OnRoundStarted;
            EventHandlers = null;
            Instance = null;

            base.OnDisabled();
        }
    }
}
