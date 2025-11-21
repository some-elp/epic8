using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Skills
{
    public class SkillContext
    {
        public Character User { get; }
        public List<Character> Targets { get; }
        public Skill SkillUsed { get; }

        public SkillContext(Character user, List<Character> targets, Skill skillUsed)
        {
            User = user;
            Targets = targets;
            SkillUsed = skillUsed;
        }   
    }
}
