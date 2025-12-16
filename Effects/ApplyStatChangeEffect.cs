using epic8.BuffsDebuffs;
using epic8.Calcs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{
    public class ApplyStatChangeEffect : IEffect
    {
        private readonly StatChange _statChangeTemplate;
        private readonly float _chance;
        public EffectTargetType TargetType { get; }

        public ApplyStatChangeEffect(StatChange template, EffectTargetType targetType, float chance = 1.0f)
        {
            _statChangeTemplate = template;
            _chance = chance;
            TargetType = targetType;
        }

        public void ApplyEffect(EffectContext effectContext)
        {

            foreach (Character target  in effectContext.GetTargets(TargetType))
            {
                //If we missed, don't apply the effects
                if (!(_statChangeTemplate.IsBuff))
                {
                    if (effectContext.HitResults.TryGetValue(target, out HitType hit))
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
                        if (!DebuffCalc.EffectivenessCheck(effectContext.Source, target))
                        {
                            Console.WriteLine($"{target.Name} resisted {_statChangeTemplate.Name}");
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{target.Name} is Immune to {_statChangeTemplate.Name}");
                        continue;
                    }
                }
                //Clone the statmodifiers on this buff/debuff
                StatChange clone = _statChangeTemplate.Clone(effectContext.Source);
                target.AddStatusEffect(clone);

                Console.WriteLine($"{target.Name} has been affected by {clone.Name} for {clone.Duration} turns.");
            }

        }


    }
}

