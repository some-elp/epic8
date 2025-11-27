using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.BuffsDebuffs
{
    public abstract class StatusEffect
    {
        //how long does this effect last
        public int Duration { get; set; }

        public bool IsBuff { get; }

        public bool IsDebuff { get; }

        public string Name { get; }

        public Character? AppliedBy { get; set; }

        public bool AppliedThisTurn { get; set; } = true;

        public StatusEffect(string name, int duration, bool isBuff = false, bool isDebuff = false)
        {
            Name = name;
            Duration = duration;
            IsBuff = isBuff;
            IsDebuff = isDebuff;
        }

        //Effects that happen immediately when the buff/debuff is applied
        public virtual void OnApply(Character target) { }

        //Effects that happen when the buff/debuff expires/is removed
        public virtual void OnExpire(Character target) { }

        public virtual IEnumerable<StatModifier> GetStatModifiers()
        {
            return Enumerable.Empty<StatModifier>();
        }

        public override string ToString()
        {
            return $"{Name}: {Duration} turn(s)";
        }
    }
}
