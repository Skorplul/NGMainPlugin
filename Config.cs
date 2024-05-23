using Exiled.API.Interfaces;
using System;
using System.ComponentModel;

namespace NGMainPlugin
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }

        [Description("Should Tutorials be ignored by teslas? Default: false")]
        public bool NoTesTuts { get; set; } = false;

        [Description("Should you be able to swap SCPs only one time a round? Default: true")]
        public static bool SingleSwap { get; set; } = true;

        [Description("After how many seconds should you not be able to swap scps anymore? Default: 60")]
        public static TimeSpan ScpSwapTimeout { get; set; } = TimeSpan.FromSeconds(60);

        [Description("Should SCP079 be able to use CASSI only once per round? Default: true")]
        public static bool Single079Cassi { get; set; } = true;
    }
}
