using Exiled.API.Interfaces;
using System;
using System.ComponentModel;

namespace NGMainPlugin
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }

        [Description("Should Tutorials trigger Teslagates? Default: false")]
        public bool NoTesTuts { get; set; } = false;

        [Description("After how many seconds should you not be able to swap scps anymore? Default: 60")]
        public static TimeSpan ScpSwapTimeout { get; set; } = TimeSpan.FromSeconds(60);
    }
}
