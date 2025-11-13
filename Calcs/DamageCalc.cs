using epic8.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Calcs
{
    public static class DamageCalc
    {
        public static Tuple<float, HitType> CalculateDamage(Character user, Character target, Skill skill)
        {

            /*
             * Damage Formula:
             * (attack*att_rate + health*hpScaling + defense*defScaling)*(1 + spd*speed_rate)*ele*(1.871*pow!)*skillMod/(def/300+1)*hitType 
             */

            //target's defense
            float targetDefense = target.CurrentStats.Defense;

            //the 3 basic damage scaling stats
            float attack = user.CurrentStats.Attack;
            float hp = user.CurrentStats.Hp;
            float userDefense = user.CurrentStats.Defense;

            //1.1 or 1.0
            ElementalAdvantage advantage = ElementHelper.GetElementalAdvantage(user.Element, target.Element);
            float adv = ElementHelper.GetEleAdvantageMultiplier(advantage);

            //What kind of hit did we make
            HitType hitType = HitCalc.DetermineHit(user, target, advantage);
            float hitMod = HitCalc.GetHitMultiplier(hitType, user);

            //used for skills with additional damage modifiers.
            float extraMod = skill.ExtraModifier();

            float damage = (float)Math.Round((attack*skill.atkRate+hp*skill.hpScaling+userDefense*skill.defScaling)
                *extraMod*adv*(1.871*skill.power)*skill.damageMod/(targetDefense/300+1)*hitMod);
            return Tuple.Create(damage, hitType);
        }
    }
}
