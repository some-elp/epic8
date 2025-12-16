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
        AliveAllies,
        AliveEnemies,
        AllAllies,
        AlliesExceptSelf,
        RandomEnemy,
        RandomAlly,
        TwoEnemy,

    }
    public interface IEffect
    {
        EffectTargetType TargetType { get; }
        void ApplyEffect(EffectContext effectContext);
    }
}
