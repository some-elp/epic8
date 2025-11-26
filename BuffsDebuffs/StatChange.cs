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
        public StatChange(string name, int duration, bool isBuff = false, bool isDebuff = false,
            params StatModifier[] mods) : base(name, duration, isBuff, isDebuff)
        {
            modifiers = mods.ToList();
        }

        public override IEnumerable<StatModifier> GetStatModifiers() 
        { 
            return modifiers;
        }

        public StatChange Clone(Character appliedBy)
        {
            StatChange copy = new StatChange(Name, Duration, IsBuff, IsDebuff, modifiers.ToArray());
            copy.AppliedBy = appliedBy;
            return copy;
        }
    }
}
