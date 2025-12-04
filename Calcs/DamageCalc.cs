using epic8.Skills;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Calcs
{
    public static class DamageCalc
    {
        public static Tuple<float, HitType> CalculateDamage(SkillContext skillContext, Character target, DamageEffect dmgEffect)
        {

            /*
             * Damage Formula:
             * (attack*att_rate + health*hpScaling + defense*defScaling)*(1 + spd*speed_rate)*ele*(1.871*pow!)*skillMod/(def/300+1)*hitType 
             */

            //target's defense
            float targetDefense = target.GetEffectiveStats().Defense;

            //Account for defense pen?
            if(dmgEffect.defPen > 0)
            {
                targetDefense *= dmgEffect.defPen;
            }

            //the 3 basic damage scaling stats
            float attack = skillContext.User.GetEffectiveStats().Attack;
            float hp = skillContext.User.GetEffectiveStats().Hp;
            float userDefense = skillContext.User.GetEffectiveStats().Defense;

            //1.1 or 1.0
            ElementalAdvantage advantage = ElementHelper.GetElementalAdvantage(skillContext.User.Element, target.Element);
            float adv = ElementHelper.GetEleAdvantageMultiplier(advantage);

            //What kind of hit did we make
            HitType hitType = HitCalc.DetermineHit(skillContext.User, target, advantage);
            float hitMod = HitCalc.GetHitMultiplier(hitType, skillContext.User);

            //used for skills with additional damage modifiers.
            float extraMod = dmgEffect.UniqueDamageModFormula(skillContext, target);
            float extraFlat = dmgEffect.UniqueFlatModFormula(skillContext, target);

            //The actual damage formula
            float damage = (float)Math.Round(((attack*dmgEffect.AtkRate+hp*dmgEffect.HpScaling+userDefense*dmgEffect.DefScaling+extraFlat)
                *extraMod*adv*(1.871*dmgEffect.Power)*dmgEffect.SkillUps)/(targetDefense/300+1)*hitMod);

            //return the damage that was taken, and what kind of hit was made.
            return Tuple.Create(damage, hitType);
        }
    }
}
