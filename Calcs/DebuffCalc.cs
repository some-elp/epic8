using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Calcs
{
    public static class DebuffCalc
    {
        private static Random rng = new Random();


        //Returns true if we try to apply the debuff
        public static bool SkillRollSucceeds(float chance)
        {
            return rng.NextDouble() <= chance;
        }

        //Returns true if the debuff should be applied
        public static bool EffectivenessCheck(Character user, Character target)
        {
            float userEffectiveness = user.GetEffectiveStats().Effectiveness;
            float targetEffectResistance = target.GetEffectiveStats().EffectResistance;

            float diff = userEffectiveness - targetEffectResistance;

            float success;

            /*examples:
             * 100 eff, 100 er -> 85% chance to debuff
             * 0 eff, 0 er -> 85% chance to debuff
             * 100 eff, 40 er -> 60% chance to debuff
             * 100 eff, 10 er -> 85% chance to debuff
             * 100 eff, 200 er -> 0% chance to debuff
             * 100 eff, 15 er -> 85% chance to debuff
             */

            if (diff > 0)
            {
                success = Math.Clamp(diff, 0f, 85f) / 100f;
            }
            else
            {
                success = 0.85f + (diff / 100f);
                success = Math.Max(0f, success);
            }
                return rng.NextDouble() <= success;
        }
    }
}
