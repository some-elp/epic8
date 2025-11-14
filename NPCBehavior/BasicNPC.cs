using epic8.Skills;
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
        public (Skill, Character target) ChooseAction(Character user, List<Character> allies, List<Character> enemies)
        {
            Random rng = new Random();

            //just pick the first skill
            Skill skill = user.Skills.First();

            //grab the list of alive enemies
            List<Character> aliveEnemies = enemies.Where(e => e.isAlive).ToList();

            //randomly pick an enemy from aliveEnemies
            Character target = aliveEnemies[rng.Next(aliveEnemies.Count)];

            return (skill, target);
        }
    }
}
