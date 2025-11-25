using epic8.Units;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.BuffsDebuffs
{
    public class StatChange : IStatusEffect
    {
        public int Duration { get; set; }

        public string Name { get; }

        private readonly List<StatModifier> modifiers;

        public bool IsBuff { get; }
        public bool IsDebuff { get; }

        public Character AppliedBy { get; private set; }
        public bool AppliedThisTurn { get; set; } = true;


        public StatChange(string name, int duration, bool isBuff = false, bool isDebuff = false, params StatModifier[] mods)
        {
            Name = name;
            Duration = duration;
            modifiers = mods.ToList();
            IsBuff = isBuff;
            IsDebuff = isDebuff;
        }

        public void OnApply(Character target) { }

        public void OnExpire(Character target) { }

        public IEnumerable<StatModifier> GetStatModifiers() { return modifiers; }

        public StatChange Clone(Character appliedBy)
        {
            StatChange copy = new StatChange(Name, Duration, IsBuff, IsDebuff, modifiers.ToArray());
            copy.AppliedBy = appliedBy;
            return copy;
        }
    }
}
