using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using epic8.Skills;

namespace epic8.Field
{
    public class BattleContext
    {
        public List<Character> allUnits { get; }

        public event Action<Character, Skill>? onSkillUsed;

        public BattleContext(List<Character> units)
        {
            allUnits = units;
        }

        public void RegisterCharacter(Character c)
        {
            allUnits.Add(c);
        }
    }
}
