using epic8.BuffsDebuffs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{
    public class ApplyCounterattackBuff : IEffect
    {
        private readonly int _duration;
        public EffectTargetType TargetType { get; }

        public ApplyCounterattackBuff(EffectTargetType targetType, int duration)
        {
            TargetType = targetType;
            _duration = duration;
        }

        public void ApplyEffect(EffectContext effectContext)
        {
            foreach(Character target in effectContext.GetTargets(TargetType))
            {
                target.AddStatusEffect(new CounterattackBuff(_duration, effectContext.Source));
                Console.WriteLine($"{target.Name} has received Counterattack buff for {_duration} turns.");
            }
        }
    }
}
