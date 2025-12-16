using epic8.Field;
using epic8.Effects;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.NPCBehavior
{
    public interface INPCController
    {
        (Skill, Character target) ChooseAction(Character user, BattleContext context);
    }
}
