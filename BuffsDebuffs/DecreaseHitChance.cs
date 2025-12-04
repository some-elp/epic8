using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.BuffsDebuffs
{
    public class DecreaseHitChance : StatusEffect
    {

        public DecreaseHitChance(int duration, Character appliedBy)
            : base("Decrease Hit Chance", duration, "Decrease Hit Chance", false, TickTime.EndOfTurn)
        {
            AppliedBy = appliedBy;
        }
    }
}
