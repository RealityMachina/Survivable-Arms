using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleTech;
using Harmony;

namespace SurvivableArms
{
    class ArmNukeChecker
    {
        [HarmonyPatch(typeof(BattleTech.MechComponent))]
        [HarmonyPatch("DamageComponent")]
        public static class BattleTech_MechComponent_Prefix
        {
            static void Prefix(MechComponent __instance, ref ComponentDamageLevel damageLevel, bool applyEffects)
            {
                if(__instance.mechComponentRef.MountedLocation == ChassisLocations.LeftArm && Holder.LeftArmSurvived)
                {
                    if(damageLevel == ComponentDamageLevel.Destroyed)
                    {
                        damageLevel = ComponentDamageLevel.NonFunctional;
                    }
                }

                if (__instance.mechComponentRef.MountedLocation == ChassisLocations.RightArm && Holder.RightArmSurvived)
                {
                    if (damageLevel == ComponentDamageLevel.Destroyed)
                    {
                        damageLevel = ComponentDamageLevel.NonFunctional;
                    }
                }
            }
        }
    }
}
