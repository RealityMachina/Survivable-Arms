using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;

namespace SurvivableArms
{
    class Holder
    {
        public static bool LeftArmSurvived; //if this is set to true, a side torso just exploded while the arm was intact
        public static bool RightArmSurvived;

        public static void Reset()
        {
            LeftArmSurvived = false;
            RightArmSurvived = false;
        }

        [HarmonyPatch(typeof(BattleTech.GameInstance), "LaunchContract", new Type[] { typeof(Contract), typeof(string) })]
        public static class BattleTech_GameInstance_HolderLaunchContract_Patch
        {
            static void Postfix()
            {
                // reset on new contracts
                Reset();
            }
        }
    }
}
