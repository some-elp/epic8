using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Skills
{

    public enum EffectTargetType
    {
        Self,
        SkillTarget,
        AllAllies,
        AllEnemies,
        RandomEnemy,
        RandomAlly
    }
    public interface ISkillEffect
    {
        EffectTargetType TargetType { get; }
        void ApplyEffect(SkillContext skillContext);
    }
}
