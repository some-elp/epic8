using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{
    public class GainExtraTurnEffect : IEffect
    {
        public EffectTargetType TargetType { get; }
        public GainExtraTurnEffect(EffectTargetType targetType)
        {
            TargetType = targetType;
        }

        public void ApplyEffect(EffectContext effectContext)
        {
            foreach (Character target in effectContext.GetTargets(TargetType))
            {
                target.ExtraTurns++;
                Console.WriteLine($"{target.Name} gains an Extra Turn.");
            }
        }

    }
}
