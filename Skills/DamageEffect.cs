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

        public DamageEffect(EffectTargetType targetType)
        {
            TargetType = targetType;
        }

        public void ApplyEffect(SkillContext skillContext)
        {
            //item1 = damage, item2 = hitType
            foreach (Character target in skillContext.GetTargets(TargetType))
            {
                //Grab how much damage we did to this target, and what kind of hit we made
                Tuple<float, HitType> tuple = DamageCalc.CalculateDamage(skillContext.User, target, skillContext.SkillUsed);
                if (tuple.Item2 == HitType.Miss)
                    Console.WriteLine($"{skillContext.User.Name} has missed on {target.Name}!");
                if (tuple.Item2 == HitType.Critical)
                    Console.WriteLine($"{skillContext.User.Name} scores a critical hit on {target.Name}!");
                if (tuple.Item2 == HitType.Crushing)
                    Console.WriteLine($"{skillContext.User.Name} scores a crushing hit on {target.Name}!");
                target.TakeDamage(tuple.Item1);
            }

        }
    }
}
