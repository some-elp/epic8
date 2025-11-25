using epic8.Units;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.BuffsDebuffs
{
    public class StatModifier
    {
        /*
         * This class holds 2 things:
         * 1. What stat is being modified
         * 2. How much that stat is being modified
         */

        public StatType Stat;
        public float PercentChange;
        public float FlatChange;

        public StatModifier(StatType stat, float percentChange = 0, float flatChange = 0)
        {
            Stat = stat;
            PercentChange = percentChange;
            FlatChange = flatChange;
        }
    }
}
