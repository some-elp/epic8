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
    public class ApplyDecreaseHitChance : ISkillEffect
    {
        public EffectTargetType TargetType { get; }
        private readonly int _duration;
        private readonly float _chance;

        public ApplyDecreaseHitChance(int duration, EffectTargetType targetType, float chance = 1.0f)
        {
            _duration = duration;
            TargetType = targetType;
            _chance = chance;
        }

        public void ApplyEffect(SkillContext skillContext)
        {
            foreach (Character target in skillContext.GetTargets(TargetType))
            {
                if (!DebuffCalc.SkillRollSucceeds(_chance))
                {
                    //move to next target if we didn't proc the debuff
                    continue;
                }
                if (!DebuffCalc.EffectivenessCheck(skillContext.User, target))
                {
                    Console.WriteLine($"{target.Name} resisted Decrease Hit Chance");
                    return;
                }
                target.AddStatusEffect(new DecreaseHitChance(_duration, skillContext.User));
                Console.WriteLine($"{target.Name} has been affected by Decrease Hit Chance for {_duration} turns.");

            }
        }
    }
}
