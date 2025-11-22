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

        public ApplyStatChangeEffect(StatChange template, float chance = 1.0f)
        {
            _statChangeTemplate = template;
            _chance = chance;
        }

        public void ApplyEffect(SkillContext skillContext)
        {

            foreach (Character target  in skillContext.Targets)
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
                StatChange clone = new StatChange(
                    _statChangeTemplate.Name,
                    _statChangeTemplate.Duration,
                    _statChangeTemplate.IsBuff,
                    _statChangeTemplate.IsDebuff,
                    _statChangeTemplate.GetStatModifiers().ToArray());

                target.AddStatusEffect(clone);
                Console.WriteLine($"{target.Name} has been affected by {clone.Name} for {clone.Duration} turns.");
            }

        }


    }
}
