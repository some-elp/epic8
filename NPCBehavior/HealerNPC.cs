using epic8.Calcs;
using epic8.Skills;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace epic8.NPCBehavior
{
    public class HealerNPC : INPCController
    {

        /*
         * 0. Specific: Use a healing skill when relevant
         * 1. ignore invincible targets (not important for now)
         * 2. Attack target where we have elemental advantage
         * 3. Attack lowest %hp target
         * 4. Random target
         * 
         * S3 -> S2 -> S1 skill prio
         * 
         * If there are multiple units that we can hit with elemental advantage
         * Hit the one with lowest %hp.
         * If they are the same hp, select randomly between those targets
         * 
         * Future: 40% chance to hit front target, 20% for rest
         */

        private readonly Random rng = new Random();

        private readonly float _s2Threshold = 1.0f;
        private readonly float _s3Threshold = 1.0f;

        public HealerNPC(float s2Threshold, float s3Threshold)
        {
            _s2Threshold = s2Threshold;
            _s3Threshold = s3Threshold;
        }

        public (Skill, Character target) ChooseAction(Character user, List<Character> allies, List<Character> enemies)
        {
            //See if we are healing anyone.
            Skill? healSkill = ChooseHealingSkill(user, allies);

            //Are we using a healing skill?
            if(healSkill != null)
            {
                Character healTarget = ChooseHealingTarget(healSkill, allies);
                return (healSkill,  healTarget);
            }


            Skill skill = user.Skills.First();


            //grab the list of alive enemies
            List<Character> aliveEnemies = enemies.Where(e => e.isAlive).ToList();

            //list of enemies we have elemental advantage against to be populated
            List<Character> eleAdvantage = [];


            //populate list of enemies we have elemental advantage against
            foreach (Character enemy in aliveEnemies)
            {
                if (ElementHelper.GetElementalAdvantage(user.Element, enemy.Element) == ElementalAdvantage.Advantage)
                {
                    eleAdvantage.Add(enemy);
                }
            }

            if (eleAdvantage.Count > 0)
            {
                return (skill, ChooseRandom(eleAdvantage));
            }
            else
            {
                return (skill, ChooseRandom(aliveEnemies));
            }

        }

        private Character ChooseRandom(List<Character> options)
        {
            float lowestHpPercent = options.Min(c => c.CurrentHP / c.GetEffectiveStats().Hp);
            List<Character> targets = [];

            foreach (Character character in options)
            {
                if (character.CurrentHP / character.GetEffectiveStats().Hp == lowestHpPercent)
                {
                    targets.Add(character);
                }
            }

            return targets[rng.Next(targets.Count)];
        }

        private Skill? ChooseHealingSkill(Character user, List<Character> allies)
        {
            // prio S3
            if (user.Skills.Count >= 3 && user.Skills[2].CurrentCooldown <= 0
                && AnyAllyBelowThreshold(allies, _s3Threshold))
                return user.Skills[2];

            // S2
            if (user.Skills.Count >= 2 && user.Skills[1].CurrentCooldown <= 0
                && AnyAllyBelowThreshold(allies, _s2Threshold))
                return user.Skills[1];

            return null;
        }

        private bool AnyAllyBelowThreshold(List<Character> allies, float threshold)
        {
            return allies.Any(a => a.isAlive &&
                 (a.CurrentHP / a.GetEffectiveStats().Hp) < threshold);
        }

        private Character ChooseHealingTarget(Skill healingSkill, List<Character> allies)
        {
            List<Character> aliveAllies = allies.Where(a => a.isAlive).ToList();

            float lowestHpPercent = aliveAllies.Min(a => a.CurrentHP / a.GetEffectiveStats().Hp);
            var lowest = aliveAllies.Where(a => (a.CurrentHP / a.GetEffectiveStats().Hp) == lowestHpPercent).ToList();

            return lowest[rng.Next(lowest.Count)];
        }
    }
}
