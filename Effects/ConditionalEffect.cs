using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{
    public class ConditionalEffect : IEffect
    {
        public EffectTargetType TargetType { get; } = EffectTargetType.Self;

        private readonly Func<EffectContext, bool> _condition;
        private readonly IEffect _effect;

        public ConditionalEffect(Func<EffectContext, bool> condition, IEffect effect)
        {
            _condition = condition;
            _effect = effect;
        }

        public void ApplyEffect(EffectContext context)
        {
            if (_condition(context))
            {
                _effect.ApplyEffect(context);
            }
        }
    }
}
