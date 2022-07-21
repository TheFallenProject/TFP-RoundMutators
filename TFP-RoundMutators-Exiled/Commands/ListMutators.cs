#if DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using System.Reflection;

namespace TFP_RoundMutators.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class listMutator : ICommand
    {
        public string Command => "tfp_mut_list";

        public string[] Aliases => null;

        public string Description => "Overrides mutator string, forcing plugin to choose specific mutator";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Mutators: \n";
            Type[] types = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "TFP_RoundMutators_Exiled.Mutators");
            foreach (var type in types)
            {
                if (type.GetProperty("Displayname") != null)
                {
                    var refer = Activator.CreateInstance(type);
                    response += $"<b>{((bool)type.GetProperty("IsUnsafe").GetValue(refer) ? "<color=red>[UNSAFE] </color>" : "<color=green>[SAFE] </color>")}<color=yellow>{type.Name}</color></b> <= {type.GetProperty("Displayname").GetValue(refer)}\n";
                }
                else
                {
                    response += $"<b><color=red>[INVALID] {type.Name}</color></b>\n";
                }
            }
            return true;
        }

        static private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                      .ToArray();
        }
    }
}

#endif