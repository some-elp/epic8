using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Field
{
    public class Battle
    {
        private readonly List<Character> _team1;
        private readonly List<Character> _team2;

        private int _turnCount = 0;
        private TurnManager _turnManager;

        public Battle(List<Character> team1, List<Character> team2)
        {
            _team1 = team1;
            _team2 = team2;
            _turnManager = new TurnManager(_team1.Concat(_team2).ToList());

        }

        public void Start()
        {
            Console.WriteLine("--- Battle Start! ---");
            _turnManager.PrintTimeline();

            while (_team1.Any(c => c.isAlive) && _team2.Any(c => c.isAlive))
            {
                Character acting = _turnManager.GetNextTurn();
                List<Character> enemies;

                if (_team1.Contains(acting))
                {
                    enemies = _team2;
                }
                else
                {
                    enemies = _team1;
                }

                if (!acting.isAlive)
                {
                    continue;
                }

                _turnCount++;
                acting.takeTurn(enemies);
                foreach ( Character unit in _team1)
                {
                    if (unit.CurrentStats.Hp <= 0 )
                    {
                        unit.isAlive = false;
                    }
                }
                foreach (Character unit in _team2)
                {
                    if (unit.CurrentStats.Hp <= 0)
                    {
                        unit.isAlive = false;
                    }
                }
            }
            Console.WriteLine(_team1.Any(c => c.isAlive) ? "Team 1 wins." : "Team 2 wins.");
        }
    }
}
