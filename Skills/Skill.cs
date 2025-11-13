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

        public float atkRate;
        public float hpScaling;
        public float defScaling;
        public float power;

        public Skill(string name, string description, int cooldown, float atkRate, float hpScaling, float defScaling, float power)
        {
            this.Name = name;
            this.Description = description;
            this.Cooldown = cooldown;
            this.atkRate = atkRate;
            this.hpScaling = hpScaling;
            this.defScaling = defScaling;
            this.power = power;
        }

        public abstract void UseSkill(Character user, Character target);

        public abstract float ExtraModifier();
    }
}
