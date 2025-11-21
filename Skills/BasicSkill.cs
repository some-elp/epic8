using epic8.Calcs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Skills
{
    //most likely won't need this class anymore
    public class BasicSkill : Skill
    {
        public BasicSkill(string name, string description, int cooldown, float atkRate, float hpScaling, float defScaling, float power, float damageMod, TargetType targetType, List<ISkillEffect> effects) : 
            base(name, description, cooldown, atkRate, hpScaling, defScaling, power, damageMod, targetType, effects)
        {
        }

        public new float ExtraModifier(Character user, Character target)
        {
            return 1.0f;
        }

        public new void UseSkill(Character user, List<Character> targets)
        {
            //item1 = damage, item2 = hitType
            foreach (Character target in targets) {
                Tuple<float, HitType> tuple = DamageCalc.CalculateDamage(user, target, this);
                target.CurrentHP -= tuple.Item1;
                if (tuple.Item2 == HitType.Miss)
                    Console.WriteLine($"{user.Name} has missed on {target.Name}!");
                if (tuple.Item2 == HitType.Critical)
                    Console.WriteLine($"{user.Name} scores a critical hit on {target.Name}!");
                if (tuple.Item2 == HitType.Crushing)
                    Console.WriteLine($"{user.Name} scores a crushing hit on {target.Name}!");
                Console.WriteLine($"{user.Name} uses {this.Name}, Skill 1 on {target.Name} for {tuple.Item1} damage.");
                Console.WriteLine($"{target.Name} has {target.CurrentHP} HP remaining.");
            }

            CurrentCooldown = Cooldown;
        }
    }
}
