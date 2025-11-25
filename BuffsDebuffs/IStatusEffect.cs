using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.BuffsDebuffs
{
    public interface IStatusEffect
    {
        //how long does this effect last
        int Duration { get; set; }

        bool IsBuff { get; }

        bool IsDebuff { get; }

        string Name { get; }

        //Effects that happen immediately when the buff/debuff is applied
        void OnApply(Character target);

        //Effects that happen when the buff/debuff expires/is removed
        void OnExpire(Character target);

        IEnumerable<StatModifier> GetStatModifiers();
    }
}
