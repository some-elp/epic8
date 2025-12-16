using epic8.BuffsDebuffs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{
    public class CleanseDebuffEffect : IEffect
    {
        private readonly int _amount;

        public EffectTargetType TargetType { get; }

        public CleanseDebuffEffect(int amount, EffectTargetType targetType)
        {
            this._amount = amount;
            TargetType = targetType;
        }

        public void ApplyEffect(EffectContext effectContext)
        {
            foreach (Character target in effectContext.GetTargets(TargetType))
            {
                //get list of X debuffs on the target, should be in application order.
                List<StatusEffect> debuffs = target.StatusEffects.Where(e => !(e.IsBuff)).Take(_amount).ToList();
                foreach(StatusEffect debuff in debuffs)
                {
                    //not sure if we need this for debuffs, or at all honestly.
                    debuff.OnExpire(target);

                    Console.WriteLine($"{debuff.Name} was cleansed from {target.Name}");

                    //remove the debuff.
                    target.StatusEffects.Remove(debuff);
                }
            }
        }
    }
}
