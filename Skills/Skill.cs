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

        //What targets is this skill hitting?
        public TargetType TargetType { get; }

        //List of the effects that this skill applies (damage counts as an effect)
        public List<ISkillEffect> Effects { get; }

        public Skill(string name, string description, int cooldown, TargetType targetType, List<ISkillEffect> effects)
        {
            Name = name;
            Description = description;
            Cooldown = cooldown;
            TargetType = targetType;
            Effects = effects;
        }

        public void UseSkill(Character user, Character target, BattleContext context)
        {
            //useful to pass information around
            SkillContext skillContext = new SkillContext(user, target, this, context);

            Console.WriteLine($"{user.Name} uses {this.Name} targeting {target.Name}");

            //Go through list of this skill's effects and apply them in order
            foreach (ISkillEffect effect in Effects)
            {
                effect.ApplyEffect(skillContext);
            }

            //We have used this skill, so put it on cooldown.
            CurrentCooldown = Cooldown;
        }
    }
}
