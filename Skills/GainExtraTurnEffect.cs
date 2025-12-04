using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Skills
{
    public class GainExtraTurnEffect : ISkillEffect
    {
        public EffectTargetType TargetType { get; }
        public GainExtraTurnEffect(EffectTargetType targetType)
        {
            TargetType = targetType;
        }

        public void ApplyEffect(SkillContext skillContext)
        {
            foreach (Character target in skillContext.GetTargets(TargetType))
            {
                target.ExtraTurns++;
                Console.WriteLine($"{target.Name} gains an Extra Turn.");
            }
        }

    }
}
