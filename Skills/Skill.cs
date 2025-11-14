using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using epic8.Field;

namespace epic8.Skills
{

    public enum TargetType
    {
        SingleEnemy,
        AllEnemies,
        SingleAlly,
        AllAllies,
        Self
    }

    public abstract class Skill
    {
        public string Name { get; }
        public string Description { get; }
        public int Cooldown { get; }
        public int CurrentCooldown { get; }

        public float AtkRate;
        public float HpScaling;
        public float DefScaling;
        public float Power;
        public float DamageMod;
        public TargetType TargetType { get; }

        public Skill(string name, string description, int cooldown, float atkRate, float hpScaling, float defScaling, float power, float damageMod, TargetType targetType)
        {
            Name = name;
            Description = description;
            Cooldown = cooldown;
            AtkRate = atkRate;
            HpScaling = hpScaling;
            DefScaling = defScaling;
            Power = power;
            DamageMod = damageMod;
            TargetType = targetType;
        }

        public abstract void UseSkill(Character user, Character target);

        public abstract float ExtraModifier();
    }
}
