using epic8.BuffsDebuffs;
using epic8.Calcs;
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
        private readonly StatChange _statChangeTemplate;
        private readonly float _chance;
        public EffectTargetType TargetType { get; }

        public ApplyStatChangeEffect(StatChange template, EffectTargetType targetType, float chance = 1.0f)
        {
            _statChangeTemplate = template;
            _chance = chance;
            TargetType = targetType;
        }

        public void ApplyEffect(SkillContext skillContext)
        {

            foreach (Character target  in skillContext.GetTargets(TargetType))
            {
                if (_statChangeTemplate.IsDebuff)
                {
                    if (!DebuffCalc.SkillRollSucceeds(_chance))
                    {
                        //move to next target if we didn't proc the debuff
                        continue;
                    }
                    if(!DebuffCalc.EffectivenessCheck(skillContext.User, target))
                    {
                        Console.WriteLine($"{target.Name} resisted {_statChangeTemplate.Name}");
                        return;
                    }
                }
                //Clone the statmodifiers on this buff/debuff
                StatChange clone = _statChangeTemplate.Clone(skillContext.User);
                target.AddStatusEffect(clone);

                Console.WriteLine($"{target.Name} has been affected by {clone.Name} for {clone.Duration} turns.");
            }

        }


    }
}
