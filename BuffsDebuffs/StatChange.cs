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

        public string Name;

        private readonly List<StatModifier> modifiers;

        public bool IsBuff { get; }
        public bool IsDebuff { get; }

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
    }
}
