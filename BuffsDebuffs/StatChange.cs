using epic8.Units;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.BuffsDebuffs
{
    public class StatChange : StatusEffect
    {

        private readonly List<StatModifier> modifiers;

        //base class takes care of all of these parameters except for stat modifiers
        public StatChange(string name, int duration, string category, bool isBuff, TickTime tickTime, List<StatModifier> mods, 
            bool isUndispellable = false, int priority = 0) : base(name, duration, category, isBuff, tickTime, isUndispellable, priority)
        {
            modifiers = mods;
        }

        public override IEnumerable<StatModifier> GetStatModifiers()
        { 
            return modifiers;
        }

        public StatChange Clone(Character appliedBy)
        {
            StatChange copy = new StatChange(Name, Duration, Category, IsBuff, TickTime, modifiers, IsUndispellable, Priority);
            copy.AppliedBy = appliedBy;
            return copy;
        }
    }
}
