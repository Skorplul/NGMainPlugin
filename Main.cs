using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server = Exiled.Events.Handlers.Server;
using Player = Exiled.Events.Handlers.Player;
using Map = Exiled.Events.Handlers.Map;
using SCP079 = Exiled.Events.Handlers.Scp079;

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
        public  static TimeSpan SwapTimeout = Config.ScpSwapTimeout;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers();
            Player.TriggeringTesla += EventHandlers.OnTriggeringTesla;
            SCP079.GainingLevel += EventHandlers.OnSCP079GainingLvl;
            Server.RoundStarted += EventHandlers.OnRoundStarted;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.TriggeringTesla -= EventHandlers.OnTriggeringTesla;
            SCP079.GainingLevel -= EventHandlers.OnSCP079GainingLvl;
            Server.RoundStarted -= EventHandlers.OnRoundStarted;
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}
