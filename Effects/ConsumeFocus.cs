using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{
    public class ConsumeFocus : IEffect
    {
        private readonly float _amount;

        public EffectTargetType TargetType { get; }

        public ConsumeFocus(float amount, EffectTargetType targetType)
        {
            _amount = amount;
            TargetType = targetType;
        }

        public void ApplyEffect(EffectContext effectContext)
        {
            foreach (Character target in effectContext.GetTargets(TargetType))
            {
                if (target.Focus >= _amount)
                {
                    target.Focus -= _amount;
                    effectContext.ConsumedFocus = true;
                    Console.WriteLine($"{target.Name} consumed {_amount} Focus.");
                }
            }
        }
    }
}
