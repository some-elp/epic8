using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.BuffsDebuffs
{
    public class Provoke : StatusEffect
    {
        public Provoke(int duration, Character appliedBy)
    : base("Provoke", duration, "Provoke", false, TickTime.EndOfTurn)
        {
            AppliedBy = appliedBy;
        }
    }
}
