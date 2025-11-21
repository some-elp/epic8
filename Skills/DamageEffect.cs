using epic8.Calcs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Skills
{
    public class DamageEffect : ISkillEffect
    {
        public EffectTargetType TargetType { get; set; }

        public DamageEffect() { }

        public void ApplyEffect(SkillContext skillContext)
        {
            //item1 = damage, item2 = hitType
            foreach (Character target in skillContext.Targets)
            {
                Tuple<float, HitType> tuple = DamageCalc.CalculateDamage(skillContext.User, target, skillContext.SkillUsed);
                target.CurrentHP -= tuple.Item1;
                if (tuple.Item2 == HitType.Miss)
                    Console.WriteLine($"{skillContext.User.Name} has missed on {target.Name}!");
                if (tuple.Item2 == HitType.Critical)
                    Console.WriteLine($"{skillContext.User.Name} scores a critical hit on {target.Name}!");
                if (tuple.Item2 == HitType.Crushing)
                    Console.WriteLine($"{skillContext.User.Name} scores a crushing hit on {target.Name}!");
                Console.WriteLine($"{skillContext.User.Name} uses {skillContext.SkillUsed.Name} on {target.Name} for {tuple.Item1} damage.");
                Console.WriteLine($"{target.Name} has {target.CurrentHP} HP remaining.");
            }

            skillContext.SkillUsed.CurrentCooldown = skillContext.SkillUsed.Cooldown;
        }
    }
}
