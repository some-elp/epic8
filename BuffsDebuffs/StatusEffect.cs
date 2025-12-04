using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.BuffsDebuffs
{
    public enum TickTime
    {
        StartOfTurn,
        EndOfTurn
    }

    public abstract class StatusEffect
    {
        //how long does this effect last
        public int Duration { get; set; }

        //is this a buff or debuff
        public bool IsBuff { get; }

        //usually false, used for undispellable buffs
        public bool IsUndispellable { get; }

        //name of the buff (ex. Increase Attack)
        public string Name { get; }
        
        //maybe not useful
        public Character? AppliedBy { get; set; }

        //Was this debuff applied this turn?
        public bool AppliedThisTurn { get; set; } = true;

        //used to determine if we already have this type of buff (GAB and Increase Attack are the same)
        public string Category { get; set; }

        //used to determine which buff to keep when 2 buffs have the same category
        public int Priority { get; set; }

        public TickTime TickTime { get; set; }

        public StatusEffect(string name, int duration, string category, bool isBuff, TickTime tickTime, bool isUndispellable = false, int priority = 0)
        {
            Name = name;
            Duration = duration;
            Category = category;
            IsBuff = isBuff;
            TickTime = tickTime;
            IsUndispellable = isUndispellable;
            Priority = priority;
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
