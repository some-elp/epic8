using epic8.BuffsDebuffs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Skills
{
    public class ApplyStatChangeEffect : ISkillEffect
    {
        private readonly StatChange statChangeTemplate;
        public EffectTargetType TargetType { get; }

        public ApplyStatChangeEffect(StatChange template)
        {
            statChangeTemplate = template;
        }

        public void ApplyEffect(SkillContext skillContext)
        {

            foreach (Character target  in skillContext.Targets)
            {
                StatChange clone = new StatChange(
                    statChangeTemplate.Name,
                    statChangeTemplate.Duration,
                    statChangeTemplate.IsBuff,
                    statChangeTemplate.IsDebuff,
                    statChangeTemplate.GetStatModifiers().ToArray());

                target.AddStatusEffect(clone);
                Console.WriteLine($"{target.Name} has been affected by {clone.Name} for {clone.Duration} turns.");
            }

        }


    }
}
