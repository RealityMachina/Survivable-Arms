using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;

namespace SurvivableArms
{
    public class InitClass
    {
        public static void Init()
        {
            var harmony = HarmonyInstance.Create("Battletech.realitymachina.SurvivableArms");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
