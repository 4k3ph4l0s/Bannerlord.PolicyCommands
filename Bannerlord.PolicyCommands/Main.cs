using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.PolicyCommands
{
    public class Main : MBSubModuleBase
    {
        [CommandLineFunctionality.CommandLineArgumentFunction("enact_policy", "campaign")]
        private static string EnactPolicy(List<string> strings)
        {
            return switchPolicy(strings, true);
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("abolish_policy", "campaign")]
        private static string AbolishPolicy(List<string> strings)
        {
            return switchPolicy(strings, false);
        }

        private static string switchPolicy(List<string> strings, bool active)
        {
                if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
                return CampaignCheats.ErrorType;

            if (!CampaignCheats.CheckParameters(strings, 2) || CampaignCheats.CheckHelp(strings))
                return "Format uses kingdom and policy ID parameters without spaces: campaign.enact_policy [kingdom] [policy]";

            var kingdomStr = strings[0].ToLower();
            var policyStr = strings[1].ToLower();

            Kingdom? kingdom = null;
            PolicyObject? policy = null;

            foreach (var k in Kingdom.All)
            {
                var id = k.Name.ToString().ToLower().Replace(" ", "");

                if (id == kingdomStr)
                    kingdom = k;
            }

            foreach (var p in PolicyObject.All)
            {
                var id = p.Name.ToString().ToLower().Replace(" ", "");

                if (id == policyStr)
                    policy = p;
            }

            if (kingdom is null)
                return "Could not find required kingdom! Format uses kingdom ID parameter without spaces.";

            if (policy is null)
                return "Could not find required policy! Format uses policy ID parameter without spaces.";

            if (active)
            {
                // enact
                if (kingdom.ActivePolicies.Contains(policy))
                    return $"Policy {policy.Name} is already active.";

                kingdom.AddPolicy(policy);
                return $"Policy {policy.Name} enacted!";
            }
            else
            {
                // abolish
                if (!kingdom.ActivePolicies.Contains(policy))
                    return $"Policy {policy.Name} is not active.";

                kingdom.RemovePolicy(policy);
                return $"Policy {policy.Name} abolished!";
            }
        }
    }
}
