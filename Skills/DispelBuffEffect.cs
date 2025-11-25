using epic8.BuffsDebuffs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Skills
{
    public class DispelBuffEffect : ISkillEffect
    {
        private readonly int _amount;
        public EffectTargetType TargetType { get; }

        public DispelBuffEffect(int amount, EffectTargetType targetType)
        {
            _amount = amount;
            TargetType = targetType;
        }

        public void ApplyEffect(SkillContext skillContext)
        {
            foreach (Character target in skillContext.GetTargets(TargetType))
            {
                //get list of X buffs on the target, should be in application order.
                List<IStatusEffect> buffs = target.StatusEffects.Where(e => e.IsBuff).Take(_amount).ToList();

                foreach (IStatusEffect buff in buffs)
                {
                    //not sure if we need this for debuffs, or at all honestly.
                    buff.OnExpire(target);

                    Console.WriteLine($"{buff.Name} was dispelled from {target.Name}");

                    //remove the debuff.
                    target.StatusEffects.Remove(buff);
                }
            }
        }
    }
}
