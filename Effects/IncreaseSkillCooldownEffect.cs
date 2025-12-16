using epic8.Calcs;
using epic8.Units;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{
    public class IncreaseSkillCooldownEffect : IEffect
    {
        private int _amount;
        private float _chance;

        public EffectTargetType TargetType { get; }

        public IncreaseSkillCooldownEffect(int amount, EffectTargetType targetType, float chance = 1.0f)
        {
            _amount = amount;
            TargetType = targetType;
            _chance = chance;
        }

        public void ApplyEffect(EffectContext effectContext)
        {
            foreach (Character target in effectContext.GetTargets(TargetType))
            {
                //If we missed, don't apply the effects.
                if (effectContext.HitResults.TryGetValue(target, out HitType hit))
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
                if (!target.IsImmune())
                {
                    //move to next target if effect was resisted.
                    if (!DebuffCalc.EffectivenessCheck(effectContext.Source, target))
                    {
                        Console.WriteLine($"{target.Name} resisted Skill Cooldown Increase.");
                        continue;
                    }
                }
                else
                {
                    //move to next target if target is immune to the effect
                    Console.WriteLine($"{target.Name} is immune to Skill Cooldown Increase.");
                    continue;
                }
                //increase cd of every skill except for s1
                for (int i = 1; i < target.Skills.Count; i++)
                {
                    target.Skills[i].CurrentCooldown += _amount;
                    //limit cooldown between 0 and the skill's max cooldown.
                    Math.Clamp(target.Skills[i].CurrentCooldown, 0, target.Skills[i].Cooldown);
                }
                Console.WriteLine($"{target.Name}'s Skill Cooldowns were increased by {_amount} turns.");
            }
        }
    }
}
