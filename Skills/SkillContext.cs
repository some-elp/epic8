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

        public List<Character> Allies { get; }
        public List<Character> Enemies { get; }

        public SkillContext(Character user, Character target, Skill skillUsed, List<Character> allies, List<Character> enemies)
        {
            User = user;
            Target = target;
            SkillUsed = skillUsed;
            Allies = allies;
            Enemies = enemies;
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
                    return Allies.Where(a => a.isAlive).ToList();
                case EffectTargetType.AllEnemies:
                    return Enemies.Where(e => e.isAlive).ToList();
                default:
                    throw new NotImplementedException($"Target type {effectTargetType} not implemented.");
            }
        }
    }
}
