using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.BuffsDebuffs
{
    public class Unhealable : StatusEffect
    {
        public Unhealable(int duration, Character appliedBy)
            : base("Unhealable", duration, "Unhealable", false, TickTime.EndOfTurn)
        {
            AppliedBy = appliedBy;
        }
    }
}
