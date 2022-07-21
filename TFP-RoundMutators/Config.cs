using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFP_RoundMutators
{
    public class Config : Qurre.API.Addons.IConfig
    {
        [Description("DO NOT CHANGE!")]
        public string Name { get; set; } = "TFP-RM";
       
        [Description("All unsafe mutators will be not used if set to false. Highly unreccomended to turn it on during production!")]
        public bool UnsafeMode { get; set; } = false;
    }
}
