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
            response = "Mutators: ";
            Type[] types = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "TFP_RoundMutators_Exiled.Mutators");
            foreach (var type in types)
            {
                var refer = Activator.CreateInstance(type);
                response += $"<b><color=yellow>{type.Name}</color></b> <= {type.GetField("Displayname").GetValue(refer)}\n";
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