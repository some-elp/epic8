using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.BuffsDebuffs
{
    public class BarrierBuff : StatusEffect
    {

        public float Remaining { get; set; }

        public BarrierBuff(int duration, float amount, Character appliedBy)
            : base("Barrier", duration, "Barrier", true, TickTime.EndOfTurn)
        {
            Remaining = amount;
            AppliedBy = appliedBy;
        }

        public float AbsorbDamage(float damage)
        {
            float absorbed = Math.Min(Remaining, damage);
            Remaining -= absorbed;
            return absorbed;
        }

        public override string ToString()
        {
            return $"{Name}: Amount Remaining - {Remaining} - {Duration} turn(s)";

        }
    }
}
