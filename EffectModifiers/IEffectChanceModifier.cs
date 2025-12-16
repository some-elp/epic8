using epic8.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.EffectModifiers
{
    public interface IEffectChanceModifier
    {
        float ModifyEffect(IEffect effect, EffectContext effectContext, float baseChance);
    }
}
