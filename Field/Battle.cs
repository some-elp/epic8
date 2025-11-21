using epic8.Units;
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
            //Start of battle
            _turnManager.PrintTimeline();

            //Keep looping while both teams have at least 1 unit alive
            while (_team1.Any(c => c.isAlive) && _team2.Any(c => c.isAlive))
            {
                //Get next acting unit
                Character acting = _turnManager.GetNextTurn();
                List<Character> enemies;
                List<Character> allies;

                //Decide which group is considered the enemies
                if (_team1.Contains(acting))
                {
                    enemies = _team2;
                    allies = _team1;
                }
                else
                {
                    enemies = _team1;
                    allies = _team2;
                }

                //Loop again if we somehow picked up a dead unit to take the turn.
                if (!acting.isAlive)
                {
                    continue;
                }

                //Acting unit takes its turn.
                acting.takeTurn(enemies, allies);

                //Check to see if any units have died after this turn.
                foreach ( Character unit in _team1)
                {
                    if (unit.CurrentHP <= 0 )
                    {
                        unit.isAlive = false;
                    }
                }
                foreach (Character unit in _team2)
                {
                    if (unit.CurrentHP <= 0)
                    {
                        unit.isAlive = false;
                    }
                }
            }
            //After one team has been defeated, display the winning team.
            Console.WriteLine(_team1.Any(c => c.isAlive) ? "Team 1 wins." : "Team 2 wins.");
        }
    }
}
