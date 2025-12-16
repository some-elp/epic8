using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{
    public class CRPushEffect : IEffect
    {
        private readonly float _amount;

        public EffectTargetType TargetType { get; }

        public CRPushEffect(float amount, EffectTargetType targetType)
        {
            _amount = Math.Max(amount, 0f);
            TargetType = targetType;
        }

        public void ApplyEffect(EffectContext effectContext)
        {
            foreach (Character target in effectContext.GetTargets(TargetType))
            {
                target.CRMeter += _amount;

                Console.WriteLine($"{target.Name} has been CR Pushed by {_amount*100f}%");
            }
        }
    }
}
