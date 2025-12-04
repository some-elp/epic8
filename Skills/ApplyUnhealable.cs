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
    public class ApplyUnhealable : ISkillEffect
    {
        private readonly int _duration;
        private readonly float _chance;
        public EffectTargetType TargetType { get; }


        public ApplyUnhealable(int duration, EffectTargetType targetType, float chance)
        {
            _duration = duration;
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
                    //move to next target if we didn't proc the debuff
                    continue;
                }
                if (!target.IsImmune())
                {
                    if (!DebuffCalc.EffectivenessCheck(skillContext.User, target))
                    {
                        Console.WriteLine($"{target.Name} resisted Unhealable");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine($"{target.Name} is immune to Unhealable");
                    continue;
                }
                target.AddStatusEffect(new Unhealable(_duration, skillContext.User));
                Console.WriteLine($"{target.Name} has been affected by Unhealable for {_duration} turns.");

            }
        }
    }
}
