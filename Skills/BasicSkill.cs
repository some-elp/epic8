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
        public BasicSkill(string name, string description, int cooldown) : base(name, description, cooldown)
        {
        }

        public override void UseSkill(Character user, Character target)
        {
            float damage = 10.0f;
            target.CurrentStats.Hp -= damage;
            Console.WriteLine($"{user.Name} uses Basic Skill on {target.Name} for {damage} damage.");
            Console.WriteLine($"{target.Name} has {target.CurrentStats.Hp} HP remaining.");
        }
    }
}
