using epic8.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.NPCBehavior
{
    public interface INPCController
    {
        (Skill, Character target) ChooseAction(Character user, List<Character> allies, List<Character> enemies);
    }
}
