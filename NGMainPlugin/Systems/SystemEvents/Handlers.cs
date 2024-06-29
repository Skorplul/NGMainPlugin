using Exiled.Events.EventArgs.Server;

namespace NGMainPlugin.Systems.SystemEvents
{
    public class Handlers
    {
        //Event commands params
        public bool NoRespawn = false;
        public bool NoAutoNuke = false;

        public void Spawning(RespawningTeamEventArgs ev)
        {
            if (NoRespawn)
                ev.IsAllowed = false;
        }
    }
}
