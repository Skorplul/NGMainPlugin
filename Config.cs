using Exiled.API.Interfaces;
using System;
using System.ComponentModel;

namespace NGMainPlugin
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }

        [Description("Should Tutorials trigger Teslagates? Defult: false")]
        public bool NoTesTuts { get; set; } = false;

        [Description("After how many seconds should you not be able to swap scps anymore? Defult: 60 IT DOESN'T WORK YET!!!!")]
        public static TimeSpan ScpSwapTimeout { get; set; } = TimeSpan.FromSeconds(60);
    }
}
