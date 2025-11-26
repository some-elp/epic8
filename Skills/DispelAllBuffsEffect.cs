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
    public class DispelAllBuffsEffect
    {
        public EffectTargetType TargetType { get; }
        private readonly float _chance;

        public DispelAllBuffsEffect(EffectTargetType targetType, float chance = 1.0f)
        {
            TargetType = targetType;
            _chance = chance;
        }

        public void ApplyEffect(SkillContext skillContext)
        {
            foreach (Character target in skillContext.GetTargets(TargetType))
            {
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
                //get list of all buffs on the target, should be in application order.
                List<IStatusEffect> buffs = target.StatusEffects.Where(e => e.IsBuff).ToList();
                foreach (IStatusEffect buff in buffs)
                {
                    //not sure if we need this for debuffs, or at all honestly.
                    buff.OnExpire(target);

                    //remove the debuff.
                    target.StatusEffects.Remove(buff);
                }
                Console.WriteLine($"{skillContext.User.Name} has dispelled all buffs from {target.Name}");
            }
        }
    }
}
