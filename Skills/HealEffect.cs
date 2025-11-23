using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Skills
{
    public class HealEffect : ISkillEffect
    {
        public EffectTargetType TargetType { get; }

        private readonly Func<SkillContext, Character, float> _healFormula;

        private readonly float _hpMulti;

        public HealEffect(Func<SkillContext, Character, float> healFormula, EffectTargetType targetType)
        {
            _healFormula = healFormula;
            TargetType = targetType;
        }

        public void ApplyEffect(SkillContext skillContext)
        {
            foreach (Character target in skillContext.GetTargets(TargetType))
            {
                float amount = _healFormula(skillContext, target);
                target.CurrentHP += amount;
                target.CurrentHP = Math.Max(target.CurrentHP, target.GetEffectiveStats().Hp);
                Console.WriteLine($"{target} was healed for {amount}");
            }
        }
    }
}
