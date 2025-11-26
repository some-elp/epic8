using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Skills
{
    public class CRPushEffect : ISkillEffect
    {
        private readonly float _amount;

        public EffectTargetType TargetType { get; }

        public CRPushEffect(float amount, EffectTargetType targetType)
        {
            _amount = Math.Clamp(amount, 0f, 1.0f);
            TargetType = targetType;
        }

        public void ApplyEffect(SkillContext skillContext)
        {
            foreach (Character target in skillContext.GetTargets(TargetType))
            {
                target.CRMeter += _amount;

                Console.WriteLine($"{target.Name} has been CR Pushed by {_amount*100f}%");
            }
        }
    }
}
