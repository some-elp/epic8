using epic8.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.EffectModifiers
{
    public class SenyaS1ProvokeModifier : IEffectChanceModifier
    {
        public float ModifyEffect(ISkillEffect effect, SkillContext skillContext, float baseChance)
        {
            //If it is not senya's turn, +25% provoke chance
            if(effect is ApplyProvoke && skillContext.BattleContext.ActingUnit != skillContext.User
                && skillContext.SkillUsed == skillContext.User.Skills[0])
            {
                return baseChance + 0.25f;
            }
            return baseChance;
        }
    }
}
