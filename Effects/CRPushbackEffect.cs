using epic8.Calcs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{
    public class CRPushbackEffect
    {
        private readonly float _amount;
        private readonly float _chance;

        public EffectTargetType TargetType { get; }

        public CRPushbackEffect(float amount, float chance, EffectTargetType targetType)
        {
            _amount = Math.Max(amount, 0f);
            _chance = chance;
            TargetType = targetType;
        }

        public void ApplyEffect(EffectContext effectContext)
        {
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
                if (!DebuffCalc.SkillRollSucceeds(_chance))
                {
                    //move to next target if we didn't proc the debuff
                    continue;
                }
                if (!target.IsImmune())
                {
                    if (!DebuffCalc.EffectivenessCheck(effectContext.Source, target))
                    {
                        Console.WriteLine($"{target.Name} resisted CR Pushback.");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine($"{target.Name} is immune to CR Pushback.");
                    continue;
                }
                target.CRMeter -= _amount;
                target.CRMeter = Math.Max(target.CRMeter, 0f);

                Console.WriteLine($"{target.Name} has been Pushed Back by {_amount * 100f}%");
            }
        }
    }
}
