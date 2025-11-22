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
