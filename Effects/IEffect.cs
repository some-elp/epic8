using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{

    public enum EffectTargetType
    {
        Self,
        SkillTarget,
        AllAllies,
        AllEnemies,
        RandomEnemy,
        RandomAlly,
        TwoEnemy,
        AlliesExceptSelf
    }
    public interface IEffect
    {
        EffectTargetType TargetType { get; }
        void ApplyEffect(EffectContext effectContext);
    }
}
