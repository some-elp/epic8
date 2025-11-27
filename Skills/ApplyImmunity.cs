using epic8.BuffsDebuffs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Skills
{
    public class ApplyImmunity : ISkillEffect
    {
        private readonly int _duration;
        public EffectTargetType TargetType { get; }


        public ApplyImmunity(int duration, EffectTargetType targetType)
        {
            _duration = duration;
            TargetType = targetType;
        }

        public void ApplyEffect(SkillContext skillContext)
        {
            foreach (Character target in skillContext.GetTargets(TargetType))
            {
                target.AddStatusEffect(new Immunity(_duration, skillContext.User));
                Console.WriteLine($"{target.Name} has received Immunity buff for {_duration} turns.");
            }
        }
    }
}
