using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.BuffsDebuffs
{
    public class DecreaseHitChance : IStatusEffect
    {
        public int Duration { get; set; }
        public bool IsBuff { get; } = false;
        public bool IsDebuff { get; } = true;
        public string Name { get; } = "Decrease Hit Chance";
        public Character AppliedBy { get; }
        public bool AppliedThisTurn { get; set; }

        public DecreaseHitChance(int duration, Character appliedBy)
        {
            Duration = duration;
            AppliedBy = appliedBy;
        }

        public void OnApply(Character character) { }

        public void OnExpire(Character character) { }

        public IEnumerable<StatModifier> GetStatModifiers()
        {
            return Enumerable.Empty<StatModifier>();
        }

        public override string ToString()
        {
            return $"{Name}: {Duration}";
        }
    }
}
