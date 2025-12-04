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
        private readonly List<Character> _team1;
        private readonly List<Character> _team2;

        public BattleContext(List<Character> team1, List<Character> team2)
        {
            _team1 = team1;
            _team2 = team2;
        }

        public List<Character> getAlliesOf(Character character)
        {
            if (_team1.Contains(character))
            {
                return _team1;
            }
            else
            {
                return _team2;
            }
        }

        public List<Character> getEnemiesOf(Character character)
        {
            if (_team1.Contains(character))
            {
                return _team2;
            }
            else
            {
                return _team1;
            }
        }

        public void InitializePassives()
        {
            foreach(Character character in _team1.Concat(_team2))
            {

                foreach (var passive in character.Passives)
                {
                    passive.Initialize(character, this);
                }
            }
        }
    }
}
