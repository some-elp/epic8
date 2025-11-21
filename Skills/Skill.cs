using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using epic8.Field;
using epic8.Units;

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

    public class Skill
    {
        //Basic description and such
        public string Name { get; }
        public string Description { get; }

        //Cooldowns measured in turns
        public int Cooldown { get; }
        public int CurrentCooldown { get; set; }

        //Damage multipliers
        public float AtkRate;
        public float HpScaling;
        public float DefScaling;
        public float Power;
        //This one is for skill enhancements
        public float DamageMod;

        //What targets is this skill hitting?
        public TargetType TargetType { get; }

        //List of the effects that this skill applies (damage counts as an effect)
        public List<ISkillEffect> Effects { get; }

        public Skill(string name, string description, int cooldown, float atkRate, float hpScaling, float defScaling, float power, float damageMod, TargetType targetType, List<ISkillEffect> effects)
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
            Effects = effects;
        }

        public void UseSkill(Character user, List<Character> targets)
        {
            SkillContext skillContext = new SkillContext(user, targets, this);

            foreach (ISkillEffect effect in Effects)
            {
                effect.ApplyEffect(skillContext);
            }
        }

        public float ExtraModifier(Character user, Character target)
        {
            return 1.0f;
        }
    }
}
