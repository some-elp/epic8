using epic8.Field;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.PassiveSkills
{
    public abstract class PassiveSkill
    {
        public Character Owner;
        public BattleContext BattleContext;

        public void Initialize (Character owner, BattleContext battleContext)
        {
            Owner = owner;
            BattleContext = battleContext;
            Initialize();
        }

        //subscribe to relevant events
        public abstract void Initialize();

        //unsubscribe from relevant events
        public abstract void Dispose();
    }
}
