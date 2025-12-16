using epic8.BuffsDebuffs;
using epic8.Effects;
using epic8.Units;
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
        
        public static HitType DetermineHit(EffectContext effectContext, Character target, ElementalAdvantage advantage)
        {
            float missChance = 0f;

            //Add miss chance from target Evasion
            missChance += target.Evasion / 100f;

            //Add miss chance from elemental disadvantage
            if(advantage == ElementalAdvantage.Disadvantage)
            {
                missChance += 0.5f;
            }

            //+50% miss chance from decrease hit chance
            if(effectContext.Source.StatusEffects.Any(s => s is DecreaseHitChance))
            {
                missChance += 0.5f;
            }
            
            //subtract hit chance from miss chance
            missChance -= effectContext.Source.HitChance;

            if(rng.NextDouble() <= missChance)
            {
                return HitType.Miss;
            }

            //If we're attacking with advantage, bonus chances
            if(advantage == ElementalAdvantage.Advantage)
            {
                //Elemental Advantage + 15% crit chance
                if (effectContext.CanCrit && 
                    rng.NextDouble() < (effectContext.Source.GetEffectiveStats().CritChance + 15.0 - target.CriticalHitResistance) / 100.0)
                    return HitType.Critical;
                //Elemental Advantage + 50% chance of crushing hit
                if (effectContext.CanCrush && 
                    rng.NextDouble() < 0.8)
                    return HitType.Crushing;
            }
            else
            {
                //Roll our crit chance for a critical hit
                if (effectContext.CanCrit && 
                    rng.NextDouble() < (effectContext.Source.GetEffectiveStats().CritChance - target.CriticalHitResistance) / 100.0)
                    return HitType.Critical;
                //If we didn't make a crit, we can check to see if we made a crushing hit
                if (effectContext.CanCrush && 
                    rng.NextDouble() < 0.3)
                    return HitType.Crushing;
            }
            //If none of these happened, it's a normal hit
            return HitType.Normal;
        }

        public static float GetHitMultiplier(HitType hitType, Character user)
        {
            //75% damage if miss, cannot proc non-artifact effects
            if (hitType == HitType.Miss)
                return 0.75f;

            //Critical Hit Damage determined by user's CritDamage stat
            if (hitType == HitType.Critical)
                return user.GetEffectiveStats().CritDamage / 100.0f;

            //A hit with +30% damage
            if (hitType == HitType.Crushing)
                return 1.3f;

            //Regular damage
            return 1.0f;
        }
    }
}
