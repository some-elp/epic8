using epic8.BuffsDebuffs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Skills
{
    public class ApplyBarrier : ISkillEffect
    {
        private readonly Func<SkillContext, Character, float> _amountFunc;
        private readonly int _duration;
        public EffectTargetType TargetType { get; }

        public ApplyBarrier(Func<SkillContext, Character, float> amountFunc, int duration, EffectTargetType targetType)
        {
            TargetType = targetType;
            _amountFunc = amountFunc;
            _duration = duration;
        }

        public void ApplyEffect(SkillContext skillContext)
        {
            foreach (Character target in skillContext.GetTargets(TargetType))
            {
                float amount = _amountFunc(skillContext, target);
                target.AddStatusEffect(new BarrierBuff(_duration, amount));
            }
        }

    }
}
