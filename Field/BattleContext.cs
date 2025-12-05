using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using epic8.Units;

namespace epic8.Field
{
    public class BattleContext
    {
        public List<Character> Team1;
        public List<Character> Team2;

        public BattleContext(List<Character> team1, List<Character> team2)
        {
            Team1 = team1;
            Team2 = team2;
        }

        public List<Character> getAlliesOf(Character character)
        {
            if (Team1.Contains(character))
            {
                return Team1;
            }
            else
            {
                return Team2;
            }
        }

        public List<Character> getEnemiesOf(Character character)
        {
            if (Team1.Contains(character))
            {
                return Team2;
            }
            else
            {
                return Team1;
            }
        }

        public List<Character> getAllCharacters()
        {
            return Team1.Concat(Team2).ToList();
        }

        public void InitializePassives()
        {
            foreach(Character character in Team1.Concat(Team2))
            {

                foreach (var passive in character.Passives)
                {
                    passive.Initialize(character, this);
                }
            }
        }
    }
}
