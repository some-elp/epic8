using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.BuffsDebuffs
{
    public class BarrierBuff : IStatusEffect
    {
        public int Duration { get; set; }

        public bool IsBuff { get; } = true;

        public bool IsDebuff { get; } = false;

        public float Remaining { get; set; }

        public string name = "Barrier";

        public BarrierBuff(int duration, float amount)
        {
            Duration = duration;
            Remaining = amount;
        }

        public float AbsorbDamage(float damage)
        {
            float absorbed = Math.Min(Remaining, damage);
            Remaining -= absorbed;
            return absorbed;
        }

        public void OnApply(Character target)
        {

        }

        public void OnExpire(Character target)
        {

        }

        public IEnumerable<StatModifier> GetStatModifiers()
        {
            return Enumerable.Empty<StatModifier>();
        }
    }
}
