using epic8.Field;
using epic8.PassiveSkills;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.BuffsDebuffs
{
    public class CounterattackBuff : StatusEffect
    {
        private CounterattackPassive? _passive;

        public CounterattackBuff(int duration, Character appliedBy) : 
            base("Counterattack", duration, "Counterattack", true, TickTime.EndOfTurn)
        {
            AppliedBy = appliedBy;
        }

        public override void OnApply(Character target)
        {
            //add counterattack passive to the unit
            _passive = new CounterattackPassive(target);
            target.Passives.Add(_passive);
            _passive.Initialize();
        }

        public override void OnExpire(Character target)
        {
            //remove counterattack passive from the unit
            if(_passive == null)
            {
                return;
            }

            _passive.Dispose();
            target.Passives.Remove(_passive);
            _passive = null;
        }

    }
}
