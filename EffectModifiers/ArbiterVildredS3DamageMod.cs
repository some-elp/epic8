using epic8.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.EffectModifiers
{
    public class ArbiterVildredS3DamageMod : IEffectModifier
    {
        public float ModifyEffect(IEffect effect, EffectContext effectContext)
        {
            if (effect is DamageEffect && effectContext.ConsumedFocus && effectContext.SkillUsed == effectContext.Source.Skills[1])
            {
                return 0.19f;
            }
            return 0f;
        }
    }
}
