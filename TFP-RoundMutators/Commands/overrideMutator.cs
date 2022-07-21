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
    internal class overrideMutator : ICommand
    {
        public string Command => "tfp_mut_override";

        public string[] Aliases => null;

        public string Description => "Overrides mutator string, forcing plugin to choose specific mutator";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var args = arguments.ToArray();
            if (args.Length != 1)
            {
                response = $"1 Arg required (got {args.Length})";
                return false;
            }
            Type[] types = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "TFP_RoundMutators.Mutators");
            if (types.Any(pred => pred.Name == args[0]))
            {
                Plugin.overridenMutator = args[0];
                response = "Valid value recieved, overriden.";
                return true;
            }
            else
            {
                response = "Invalid name!";
                return false;
            }
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