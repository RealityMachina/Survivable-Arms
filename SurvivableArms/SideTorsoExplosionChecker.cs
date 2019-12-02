using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using UnityEngine;

namespace SurvivableArms
{
    class SideTorsoExplosionChecker
    {
        [HarmonyPatch(typeof(BattleTech.Mech))]
        [HarmonyPatch("DamageLocation")]
        public static class BattleTech_Mech_DamageLocation_Prefix
        {
            static void Prefix(Mech __instance, int originalHitLoc, WeaponHitInfo hitInfo, ArmorLocation aLoc, Weapon weapon, float totalDamage, int hitIndex, AttackImpactQuality impactQuality)
            {
                if (aLoc == ArmorLocation.None || aLoc == ArmorLocation.Invalid)
                {
                    return;
                }


                float num = totalDamage;
                float currentArmor = __instance.GetCurrentArmor(aLoc);
                if (currentArmor > 0f)
                {
                    float num2 = Math.Min(totalDamage, currentArmor);

                    num = totalDamage - num2;
                }
                if (num <= 0f)
                {
                    return; //no need to continue if the shot doesn't do anything we care about
                }
                ChassisLocations chassisLocationFromArmorLocation = MechStructureRules.GetChassisLocationFromArmorLocation(aLoc);


                float currentStructure = __instance.GetCurrentStructure(chassisLocationFromArmorLocation);
                if (currentStructure > 0f)
                {
                   
                    float num4 = Math.Min(num, currentStructure);
                    bool WasDestroyed = (currentStructure - num) <= 0; //if currentstructure minus remaining damage is less or equal to 0, then the location is destroyed.

                    num -= num4;
                    if (WasDestroyed && num4 > 0.01f) //this location was destroyed, so we now check for dependents.
                    {
                        if(chassisLocationFromArmorLocation == ChassisLocations.LeftArm)
                        {
                            Holder.LeftArmSurvived = false; //invalidate if the actual arm was destroyed

                        }
                        else if (chassisLocationFromArmorLocation == ChassisLocations.RightArm)
                        {
                            Holder.RightArmSurvived = false; //invalidate if the actual arm was destroyed

                        }
                        ChassisLocations dependentLocation = MechStructureRules.GetDependentLocation(chassisLocationFromArmorLocation);
                        if (dependentLocation != ChassisLocations.None && !__instance.IsLocationDestroyed(dependentLocation))
                        {
                            if(dependentLocation == ChassisLocations.LeftArm)
                            {
                                Holder.LeftArmSurvived = true; //side torso was destroyed, no reason the arm should be totally trashed.
                            }
                            else if (dependentLocation == ChassisLocations.RightArm)
                            {
                                Holder.RightArmSurvived = true; //side torso was destroyed, no reason the arm should be totally trashed.
                            }

                        }
                    }
                }
            }
        }
    }
}
