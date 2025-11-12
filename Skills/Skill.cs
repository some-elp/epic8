using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using epic8.Field;

namespace epic8.Skills
{
    public abstract class Skill
    {
        public string Name { get; }
        public string Description { get; }
        public int Cooldown { get; }
        public int CurrentCooldown { get; }

        public Skill(string name, string description, int cooldown)
        {
            this.Name = name;
            this.Description = description;
            this.Cooldown = cooldown;
        }

        public abstract void UseSkill(Character user, Character target);
    }
}
