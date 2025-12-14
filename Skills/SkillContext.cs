using epic8.Calcs;
using epic8.Field;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Skills
{
    public class SkillContext
    {
        public Character User { get; }
        public Character Target { get; }
        public Skill SkillUsed { get; }

        public BattleContext BattleContext { get; }

        //Keep track of what kind of hit was made on each target
        public Dictionary<Character, HitType> HitResults { get; } = new Dictionary<Character, HitType>();

        //For passives that prevent critting
        public bool CanCrit { get; set; } = true;
        public bool CanCrush { get; set; } = true;

        //For passives that automatically crit
        public bool AlwaysCrit { get; set; } = false;

        public SkillContext(Character user, Character target, Skill skillUsed, BattleContext context)
        {
            User = user;
            Target = target;
            SkillUsed = skillUsed;
            BattleContext = context;
        }

        public List<Character> GetTargets(EffectTargetType effectTargetType)
        {
            //Return the proper targets for this skill effect (Can be different from skill's target)
            switch (effectTargetType)
            {
                case EffectTargetType.Self:
                    return new List<Character> { User };
                case EffectTargetType.SkillTarget:
                    return [Target];
                case EffectTargetType.AllAllies:
                    return BattleContext.getAlliesOf(User).Where(a => a.isAlive).ToList();
                case EffectTargetType.AllEnemies:
                    return BattleContext.getEnemiesOf(User).Where(e => e.isAlive).ToList();
                default:
                    throw new NotImplementedException($"Target type {effectTargetType} not implemented.");
            }
        }
    }
}
