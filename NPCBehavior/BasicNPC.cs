using epic8.Calcs;
using epic8.Skills;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace epic8.NPCBehavior
{
    public class BasicNPC : INPCController
    {

        /* 
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

        public (Skill, Character target) ChooseAction(Character user, List<Character> allies, List<Character> enemies)
        {
            //just pick the first skill
            Skill skill = user.Skills.First();

            //loop through skills in reverse
            for(int i = user.Skills.Count; i > 0; i -= 1)
            {
                //pick the first skill that is off cooldown
                if (user.Skills[i-1].CurrentCooldown == 0)
                {
                    skill = user.Skills[i-1];
                    break;
                }
            }


            //grab the list of alive enemies
            List<Character> aliveEnemies = enemies.Where(e => e.isAlive).ToList();

            //list of enemies we have elemental advantage against to be populated
            List<Character> eleAdvantage = [];


            //populate list of enemies we have elemental advantage against
            foreach (Character enemy in aliveEnemies)
            {
                if( ElementHelper.GetElementalAdvantage(user.Element, enemy.Element) == ElementalAdvantage.Advantage)
                {
                    eleAdvantage.Add(enemy);
                }
            }

            if(eleAdvantage.Count > 0)
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
                if(character.CurrentHP / character.GetEffectiveStats().Hp == lowestHpPercent)
                {
                    targets.Add(character);
                }
            }

            return targets[rng.Next(targets.Count)];
        }
    }
}
