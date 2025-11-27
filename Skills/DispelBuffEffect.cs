using epic8.BuffsDebuffs;
using epic8.Calcs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Skills
{
    public class DispelBuffEffect : ISkillEffect
    {
        private readonly int _amount;
        private readonly float _chance;
        public EffectTargetType TargetType { get; }

        public DispelBuffEffect(int amount, EffectTargetType targetType, float chance = 1.0f)
        {
            _amount = amount;
            TargetType = targetType;
            _chance = chance;
        }

        public void ApplyEffect(SkillContext skillContext)
        {
            foreach (Character target in skillContext.GetTargets(TargetType))
            {
                //If we missed, don't apply the effects
                if (skillContext.HitResults.TryGetValue(target, out HitType hit))
                {
                    if (hit == HitType.Miss)
                    {
                        continue;
                    }
                }
                if (!DebuffCalc.SkillRollSucceeds(_chance))
                {
                    //move to next target if we didn't proc the effect
                    continue;
                }
                if (!DebuffCalc.EffectivenessCheck(skillContext.User, target))
                {
                    Console.WriteLine($"{target.Name} resisted buff dispel.");
                    return;
                }
                //get list of X buffs on the target, should be in application order.
                List<StatusEffect> buffs = target.StatusEffects.Where(e => e.IsBuff).Take(_amount).ToList();

                foreach (StatusEffect buff in buffs)
                {
                    //not sure if we need this for debuffs, or at all honestly.
                    buff.OnExpire(target);

                    Console.WriteLine($"{buff.Name} was dispelled from {target.Name}");

                    //remove the debuff.
                    target.StatusEffects.Remove(buff);
                }
            }
        }
    }
}
