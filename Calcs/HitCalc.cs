using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Calcs
{

    public enum HitType
    {
        Miss,
        Normal,
        Critical,
        Crushing
    }
    public static class HitCalc
    {
        private static Random rng = new Random();
        
        public static HitType DetermineHit(Character user, Character target, ElementalAdvantage advantage)
        {
            bool canMiss = (advantage == ElementalAdvantage.Disadvantage);

            //Elemental disadvantage miss chance for now
            if (canMiss && rng.NextDouble() <= 0.5)
                return HitType.Miss;

            //If we're attacking with advantage, bonus chances
            if(advantage == ElementalAdvantage.Advantage)
            {
                //Elemental Advantage + 15% crit chance
                if (rng.NextDouble() < (user.CurrentStats.CritChance + 15.0) / 100.0)
                    return HitType.Critical;
                //Elemental Advantage + 50% chance of crushing hit
                if (rng.NextDouble() < 0.8)
                    return HitType.Crushing;
            }
            else
            {
                if (rng.NextDouble() < (user.CurrentStats.CritChance) / 100.0)
                    return HitType.Critical;
                if (rng.NextDouble() < 0.3)
                    return HitType.Crushing;
            }
            return HitType.Normal;
        }

        public static float GetHitMultiplier(HitType hitType, Character user)
        {
            if (hitType == HitType.Miss)
                return 0.75f;
            if (hitType == HitType.Critical)
                return user.CurrentStats.CritDamage / 100.0f;
            if (hitType == HitType.Crushing)
                return 1.3f;
            return 1.0f;
        }
    }
}
