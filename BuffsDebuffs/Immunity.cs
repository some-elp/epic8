using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.BuffsDebuffs
{
    public class Immunity : StatusEffect
    {
        public Immunity(int duration, Character appliedBy)
    : base("Immunity", duration, true, false)
        {
            AppliedBy = appliedBy;
        }
    }
}
