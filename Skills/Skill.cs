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
        SingleAlly,
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
        public float SkillUps;

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
            SkillUps = damageMod;
            TargetType = targetType;
            Effects = effects;
        }

        public void UseSkill(Character user, Character target, List<Character> allies, List<Character> enemies)
        {
            SkillContext skillContext = new SkillContext(user, target, this, allies, enemies);

            Console.WriteLine($"{user.Name} uses {this.Name} targeting {target.Name}");

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
