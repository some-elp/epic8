using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace epic8.Field
{
    public class TurnManager
    {
        public List<Character> units;

        private int _turnCount = 1;

        private bool _firstPrint = true;
        public TurnManager(List<Character> characters)
        {
            units = characters;
            InitializeCRBar();
        }

        private void InitializeCRBar()
        {
            float maxSpeed = units.Max(u => u.GetEffectiveStats().Speed);

            //Set starting CRMeter of units as a proportion of the highest speed.
            foreach (Character unit in units)
            {
                unit.CRMeter = (float)unit.GetEffectiveStats().Speed / maxSpeed;
            }
        }

        public void PrintTimeline()
        {
            if (_firstPrint)
            {
                _firstPrint = false;
            }
            else
            {
                Console.WriteLine();
            }

            Console.WriteLine($"--- Turn {_turnCount} ---");
            foreach (Character unit in units.Where(character => character.isAlive).OrderBy(u => u.CRMeter))
            {
                Console.WriteLine($" - {unit}");
            }
            Console.WriteLine();
        }

        private void AdvanceTime()
        {
            float minDelta = float.MaxValue;

            foreach (Character character in units.Where(character => character.isAlive))
            {
                //Find the shortest "time" a unit needs to travel to 100%
                float delta = (1.0f - character.CRMeter) / character.GetEffectiveStats().Speed;
                if(delta < minDelta)
                {
                    minDelta = delta;
                }
            }

            foreach (Character character in units.Where((character) => character.isAlive))
            {
                //Advance all characters for the given amount of "time"
                character.CRMeter += character.GetEffectiveStats().Speed * minDelta;
            }
        }

        public Character GetNextTurn()
        {
            while (true)
            {
                //Get list of units that are at 100% CR, sorted by speed.
                List<Character> readyUnits = units.Where(u => u.isAlive && u.CRMeter >= 1.0f)
                    .OrderByDescending(u => u.GetEffectiveStats().Speed)
                    .ToList();

                //if there are any units at 100% CR
                if (readyUnits.Any())
                {
                    PrintTimeline();
                    _turnCount++;
                    //Get the first one in the list
                    Character readyUnit = readyUnits.First();
                    readyUnit.CRMeter = 0f;

                    //The other units that are at 100% get set to almost 100% to prevent two characters moving at once.
                    foreach (Character other in readyUnits.Skip(1))
                    {
                        other.CRMeter = 0.9999f;
                    }



                    return readyUnit;
                }

                //no one at 100%, advance time
                AdvanceTime();
            }

        }
    }
}
