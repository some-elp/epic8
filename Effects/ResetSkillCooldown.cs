using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{
    public class ResetSkillCooldown : IEffect
    {
        public EffectTargetType TargetType { get; }
        private int _choice;

        public ResetSkillCooldown(EffectTargetType targetType, int choice = 0)
        {
            TargetType = targetType;
            _choice = choice;
        }

        public void ApplyEffect(EffectContext effectContext)
        {
            foreach (Character target in effectContext.GetTargets(TargetType))
            {
                //Resets all skills
                if(_choice == 0)
                {
                    foreach(Skill skill in target.Skills)
                    {
                        skill.CurrentCooldown = 0;
                    }
                }
                //Reset specific skill
                else
                {
                    target.Skills[_choice].CurrentCooldown = 0;
                }
            }
        }
    }
}
