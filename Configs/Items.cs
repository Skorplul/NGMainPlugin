using NGMainPlugin.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGMainPlugin.Configs
{
    public class Items
    {
        /// <summary>
        /// Gets the list of emp greanades.
        /// </summary>
        [Description("The list of an Item")]
        public List<GrenadeLauncher> GrenadeLauncher { get; private set; } = new()
        {
            new GrenadeLauncher(),
        };
    }
}
