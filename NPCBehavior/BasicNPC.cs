using epic8.Skills;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.NPCBehavior
{
    public class BasicNPC : INPCController
    {

        //really basic AI for now

        /*
         * in future: 
         * 1. ignore invincible targets (not important for now)
         * 2. Attack target where we have elemental advantage
         * 3. Attack lowest %hp target
         * 4. Random target
         * 
         * If there are multiple units that we can hit with elemental advantage
         * Hit the one with lowest %hp.
         * If they are the same hp, select randomly between those targets
         * 
         * Future future: 40% chance to hit front target, 20% for rest
         */
        public (Skill, List<Character> target) ChooseAction(Character user, List<Character> allies, List<Character> enemies)
        {
            Random rng = new Random();

            //just pick the first skill
            Skill skill = user.Skills.First();

            //grab the list of alive enemies
            List<Character> aliveEnemies = enemies.Where(e => e.isAlive).ToList();

            //randomly pick an enemy from aliveEnemies
            List<Character> target = [aliveEnemies[rng.Next(aliveEnemies.Count)]];

            return (skill, target);
        }
    }
}
