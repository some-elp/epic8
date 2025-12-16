using epic8.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.EffectModifiers
{
    public class SenyaS1ProvokeModifier : IEffectChanceModifier
    {
        public float ModifyEffect(IEffect effect, EffectContext effectContext, float baseChance)
        {
            //If it is not senya's turn, +25% provoke chance
            if(effect is ApplyProvoke && effectContext.BattleContext.ActingUnit != effectContext.Source
                && effectContext.SkillUsed == effectContext.Source.Skills[0])
            {
                return baseChance + 0.25f;
            }
            return baseChance;
        }
    }
}
