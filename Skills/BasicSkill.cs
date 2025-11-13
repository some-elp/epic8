using epic8.Calcs;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Skills
{
    public class BasicSkill : Skill
    {
        public BasicSkill(string name, string description, int cooldown, float atkRate, float hpScaling, float defScaling, float power) : 
            base(name, description, cooldown, atkRate, hpScaling, defScaling, power)
        {
        }

        public override float ExtraModifier()
        {
            return 1.0f;
        }

        public override void UseSkill(Character user, Character target)
        {
            float damage = DamageCalc.CalculateDamage(user, target, this);
            target.CurrentHP -= damage;
            Console.WriteLine($"{user.Name} uses Basic Skill on {target.Name} for {damage} damage.");
            Console.WriteLine($"{target.Name} has {target.CurrentHP} HP remaining.");
        }
    }
}
