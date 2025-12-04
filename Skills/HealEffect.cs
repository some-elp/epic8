using epic8.BuffsDebuffs;
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

        public HealEffect(Func<SkillContext, Character, float> healFormula, EffectTargetType targetType)
        {
            _healFormula = healFormula;
            TargetType = targetType;
        }

        public void ApplyEffect(SkillContext skillContext)
        {
            foreach (Character target in skillContext.GetTargets(TargetType))
            {
                //Unhealable check
                if (target.StatusEffects.Any(e => e is Unhealable))
                {
                    Console.WriteLine($"{target.Name} cannot be healed.");
                    continue;
                }
                //Get the amount that we should heal the target for
                float amount = (float)(Math.Round(_healFormula(skillContext, target)));

                //Prevent overhealing by choosing the minimmum between our full heal and how much the target can be healed by
                float actualHealAmount = Math.Min(amount, target.GetEffectiveStats().Hp - target.CurrentHP);
                target.CurrentHP += actualHealAmount;

                Console.WriteLine($"{target.Name} was healed for {actualHealAmount}");
            }
        }
    }
}
