using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{
    public class ReviveEffect : IEffect
    {
        private readonly float _healthPercent;
        public EffectTargetType TargetType { get; }

        public ReviveEffect( float healthPercent, EffectTargetType targetType)
        {
            TargetType = targetType;
            _healthPercent = healthPercent;
        }

        public void ApplyEffect(EffectContext effectContext)
        {
            foreach(Character target in effectContext.GetTargets(TargetType))
            {
                if (!(target.isAlive))
                {
                    target.CurrentHP = target.GetEffectiveStats().Hp * _healthPercent;
                    target.isAlive = true;
                    Console.WriteLine($"{target.Name} was revived with {target.GetEffectiveStats().Hp * _healthPercent} HP");
                }
            }
        }
    }
}
