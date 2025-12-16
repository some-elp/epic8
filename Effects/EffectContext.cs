using epic8.Calcs;
using epic8.Field;
using epic8.PassiveSkills;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{
    public class EffectContext
    {
        public Character Source { get; }
        public Character? Target { get; }

        public BattleContext BattleContext { get; }

        //When effect comes from a skill
        public Skill? SkillUsed { get; }

        //When effect comes from a passive
        public Passive? PassiveSource { get; }

        //Keep track of what kind of hit was made on each target
        public Dictionary<Character, HitType> HitResults { get; } = new Dictionary<Character, HitType>();

        //For passives that prevent critting
        public bool CanCrit { get; set; } = true;
        public bool CanCrush { get; set; } = true;

        //For passives that automatically crit
        public bool AlwaysCrit { get; set; } = false;

        //cache for skill targets.
        private readonly Dictionary<EffectTargetType, List<Character>> _resolvedTargets = new();

        public EffectContext(Character source, BattleContext context, Character? target = null, Skill? skillUsed = null, Passive? passiveSource = null)
        {
            Source = source;
            Target = target;
            BattleContext = context;
            SkillUsed = skillUsed;
            PassiveSource = passiveSource;
        }

        public List<Character> GetTargets(EffectTargetType effectTargetType)
        {
            //Most useful for things like Arby S1 which hit one target + a random target.
            if (_resolvedTargets.TryGetValue(effectTargetType, out var cached))
                return cached;

            //Return the proper targets for this skill effect (Can be different from skill's target)
            switch (effectTargetType)
            {
                case EffectTargetType.Self:
                    return new List<Character> { Source };
                case EffectTargetType.SkillTarget:
                    return [Target];
                case EffectTargetType.AllAllies:
                    return BattleContext.getAlliesOf(Source).Where(a => a.isAlive).ToList();
                case EffectTargetType.AllEnemies:
                    return BattleContext.getEnemiesOf(Source).Where(e => e.isAlive).ToList();
                case EffectTargetType.TwoEnemy:
                    //Always include the skill target
                    List<Character> result = new List<Character> { Target };

                    //List of all alive enemies except target
                    List<Character> enemies = BattleContext.getEnemiesOf(Source).Where(e => e.isAlive && e != Target).ToList();

                    //If there is at least one other enemy, pick one at random
                    if (enemies.Count > 0)
                    {
                        Random rng = new Random();
                        int index = rng.Next(enemies.Count);
                        result.Add(enemies[index]);
                    }

                    return result;
                case EffectTargetType.AlliesExceptSelf:
                    return BattleContext.getAlliesOf(Source).Where(a => a.isAlive && a != Source).ToList();
                default:
                    throw new NotImplementedException($"Target type {effectTargetType} not implemented.");
            }
        }
    }
}
