using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Field
{
    public class TurnManager
    {
        public List<Character> units;

        private int _turnCount = 1;
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
            Console.WriteLine("--- Current Timeline ---");
            Console.WriteLine($"--- Turn {_turnCount} ---");
            foreach (Character unit in units)
            {
                Console.WriteLine($" - {unit}");
            }
        }

        private void AdvanceTime()
        {
            float minDelta = float.MaxValue;

            foreach (Character character in units.Where(character => character.isAlive))
            {
                float delta = (1.0f - character.CRMeter) / character.GetEffectiveStats().Speed;
                if(delta < minDelta)
                {
                    minDelta = delta;
                }
            }

            foreach (Character character in units.Where((character) => character.isAlive))
            {
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

                _turnCount++;

                //no one at 100%, advance time
                AdvanceTime();
                PrintTimeline();
            }

        }
    }
}
