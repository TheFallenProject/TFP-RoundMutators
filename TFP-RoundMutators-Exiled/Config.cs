using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFP_RoundMutators
{
    public class Config : Exiled.API.Interfaces.IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("All unsafe mutators will be not used if set to false. Highly unreccomended to turn it on during production!")]
        public bool UnsafeMode { get; set; } = false;
    }
}
