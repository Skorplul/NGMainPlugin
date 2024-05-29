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
using Exiled.CustomItems.API.Features;
using Exiled.Events.Features;

namespace NGMainPlugin
{
    public class Main : Plugin<Config>
    {
        public override string Author { get; } = "Skorp 1.0";
        public override string Name { get; } = "NGMainPlugin";
        public override string Prefix { get; } = "NGM";
        public override Version Version { get; } = new Version(0, 1, 0);

        public override Version RequiredExiledVersion { get; } = new Version(8, 5, 0);

        public EventHandlers EventHandlers;
        public static Main Instance { get; private set; }

        public override void OnEnabled()
        {
            base.OnEnabled();

            Config.LoadItems();

            Log.Debug("Registering items..");
            CustomItem.RegisterItems(overrideClass: Config.ItemConfigs);

            Instance = this;
            EventHandlers = new EventHandlers(this);

            Player.UsingItemCompleted += EventHandlers.OnTakingPainkiller;
            Player.TriggeringTesla += EventHandlers.OnTriggeringTesla;
            SCP079.GainingLevel += EventHandlers.OnSCP079GainingLvl;
            Server.RoundStarted += EventHandlers.OnRoundStarted;

            SCPSwap.Plugin = this;
            Durchsage.Plugin = this;

        }

        public override void OnDisabled()
        {
            CustomItem.UnregisterItems();

            Player.TriggeringTesla -= EventHandlers.OnTriggeringTesla;
            SCP079.GainingLevel -= EventHandlers.OnSCP079GainingLvl;
            Server.RoundStarted -= EventHandlers.OnRoundStarted;
            Player.UsingItemCompleted -= EventHandlers.OnTakingPainkiller;
            EventHandlers = null;
            Instance = null;

            base.OnDisabled();
        }
    }
}
