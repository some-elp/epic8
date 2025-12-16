using epic8.BuffsDebuffs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{
    public class ApplyBarrier : IEffect
    {
        //Function that determines how the barrier amount is calculated
        private readonly Func<EffectContext, Character, float> _amountFunc;
        private readonly int _duration;
        public EffectTargetType TargetType { get; }

        public ApplyBarrier(Func<EffectContext, Character, float> amountFunc, int duration, EffectTargetType targetType)
        {
            TargetType = targetType;
            _amountFunc = amountFunc;
            _duration = duration;
        }

        public void ApplyEffect(EffectContext effectContext)
        {
            foreach (Character target in effectContext.GetTargets(TargetType))
            {
                //Find the value of the barrier.
                float amount = (float)(Math.Round(_amountFunc(effectContext, target)));
                //Add barrier to the Character's buff/debuff list
                target.AddStatusEffect(new BarrierBuff(_duration, amount, effectContext.Source));
                Console.WriteLine($"{target.Name} has received a barrier of {amount}");
            }
        }

    }
}
