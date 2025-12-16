using epic8.BuffsDebuffs;
using epic8.Calcs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{
    public class ApplyProvoke : IEffect
    {
        public EffectTargetType TargetType { get; }
        private readonly int _duration;
        private readonly float _chance;

        public ApplyProvoke(int duration, EffectTargetType targetType, float chance = 1.0f)
        {
            _duration = duration;
            TargetType = targetType;
            _chance = chance;
        }

        public void ApplyEffect(EffectContext effectContext)
        {

            float finalChance = _chance;

            //Maybe move this from Character into the skilleffect classes?
            foreach (var mod in effectContext.Source.EffectChanceModifiers)
            {
                finalChance = mod.ModifyEffect(this, effectContext, finalChance);
            }

            foreach (Character target in effectContext.GetTargets(TargetType))
            {
                //If we missed, don't apply the effects
                if (effectContext.HitResults.TryGetValue(target, out HitType hit))
                {
                    if (hit == HitType.Miss)
                    {
                        continue;
                    }
                }
                if (!DebuffCalc.SkillRollSucceeds(finalChance))
                {
                    //move to next target if we didn't proc the debuff
                    continue;
                }
                if (!target.IsImmune())
                {
                    if (!DebuffCalc.EffectivenessCheck(effectContext.Source, target))
                    {
                        Console.WriteLine($"{target.Name} resisted Provoke");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine($"{target.Name} is immune to Provoke");
                    continue;
                }
                target.AddStatusEffect(new Provoke(_duration, effectContext.Source));
                Console.WriteLine($"{target.Name} has been affected by Provoke from {effectContext.Source.Name} for {_duration} turns.");

            }
        }
    }
}
