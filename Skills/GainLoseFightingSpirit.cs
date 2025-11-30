using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Skills
{
    public class GainLoseFightingSpirit : ISkillEffect
    {
        private readonly float _amount;
        public EffectTargetType TargetType { get; }

        public GainLoseFightingSpirit(float amount, EffectTargetType targetType)
        {
            _amount = amount;
            TargetType = targetType;
        }

        public void ApplyEffect(SkillContext skillContext)
        {
            foreach(Character target in skillContext.GetTargets(TargetType))
            {
                target.FightingSpirit += _amount;
                if (_amount >= 0) {
                    Console.WriteLine($"{target.Name} has gained {_amount} Fighting Spirit");
                }
                else
                {
                    Console.WriteLine($"{target.Name} has lost {_amount} Fighting Spirit");
                }
            }
        }
    }
}
