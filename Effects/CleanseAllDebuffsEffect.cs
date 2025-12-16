using epic8.BuffsDebuffs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{
    public class CleanseAllDebuffsEffect : IEffect
    {
        public EffectTargetType TargetType { get; }

        public CleanseAllDebuffsEffect(EffectTargetType targetType)
        {
            TargetType = targetType;
        }

        public void ApplyEffect(EffectContext effectContext)
        {
            foreach (Character target in effectContext.GetTargets(TargetType))
            {
                //get list of all debuffs on the target
                List<StatusEffect> debuffs = target.StatusEffects.Where(e => !(e.IsBuff)).ToList();
                foreach (StatusEffect debuff in debuffs)
                {
                    //not sure if we need this for debuffs, or at all honestly.
                    debuff.OnExpire(target);

                    //remove the debuff.
                    target.StatusEffects.Remove(debuff);
                }
                Console.WriteLine($"{effectContext.Source.Name} has cleansed all debuffs from {target.Name}");

            }
        }
    }
}
