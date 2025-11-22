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

        //Returns true if the debuff should be applied
        public static bool ShouldApplyDebuff(Character user, Character target, float debuffChance)
        {
            if (rng.NextDouble() > debuffChance)
            {
                return false;
            }

            float userEffectiveness = user.GetEffectiveStats().Effectiveness;
            float targetEffectResistance = target.GetEffectiveStats().EffectResistance;

            float diff = userEffectiveness - targetEffectResistance;

            diff = Math.Clamp(diff, 0f, 0.85f);

            return rng.NextDouble() <= diff;
        }
    }
}
