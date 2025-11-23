using epic8.BuffsDebuffs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Skills
{
    public class CleanseAllDebuffsEffect : ISkillEffect
    {
        public EffectTargetType TargetType { get; }

        public CleanseAllDebuffsEffect(EffectTargetType targetType)
        {
            TargetType = targetType;
        }

        public void ApplyEffect(SkillContext skillContext)
        {
            foreach (Character target in skillContext.GetTargets(TargetType))
            {
                //get list of all debuffs on the target
                List<IStatusEffect> debuffs = target.StatusEffects.Where(e => e.IsDebuff).ToList();
                foreach (IStatusEffect debuff in debuffs)
                {
                    //not sure if we need this for debuffs, or at all honestly.
                    debuff.OnExpire(target);

                    //remove the debuff.
                    target.StatusEffects.Remove(debuff);
                }
            }
        }
    }
}
