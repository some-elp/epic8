using epic8.BuffsDebuffs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{
    public class ApplyImmunity : IEffect
    {
        private readonly int _duration;
        public EffectTargetType TargetType { get; }


        public ApplyImmunity(int duration, EffectTargetType targetType)
        {
            _duration = duration;
            TargetType = targetType;
        }

        public void ApplyEffect(EffectContext effectContext)
        {
            foreach (Character target in effectContext.GetTargets(TargetType))
            {
                target.AddStatusEffect(new Immunity(_duration, effectContext.Source));
                Console.WriteLine($"{target.Name} has received Immunity buff for {_duration} turns.");
            }
        }
    }
}
