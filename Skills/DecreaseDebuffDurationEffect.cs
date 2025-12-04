using epic8.BuffsDebuffs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Skills
{
    public class DecreaseDebuffDurationEffect : ISkillEffect
    {
        private readonly int _duration;
        public EffectTargetType TargetType { get; }

        public DecreaseDebuffDurationEffect(int duration, EffectTargetType targetType)
        {
            _duration = duration;
            TargetType = targetType;
        }

        public void ApplyEffect(SkillContext skillContext)
        {
            foreach(Character target in skillContext.GetTargets(TargetType))
            {
                //get list of all debuffs on the target
                List<StatusEffect> debuffs = target.StatusEffects.Where(e => !(e.IsBuff)).ToList();
                foreach (StatusEffect debuff in debuffs)
                {
                    //subtract duration from debuffs
                    debuff.Duration -= _duration;

                    //process of removing the debuff if duration hit 0
                    if(debuff.Duration <= 0)
                    {
                        debuff.OnExpire(target);
                        target.StatusEffects.Remove(debuff);
                    }
                }
                Console.WriteLine($"All debuff durations on {target.Name} were reduced by {_duration} turns.");
            }
        }
    }
}
