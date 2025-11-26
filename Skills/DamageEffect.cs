using epic8.Calcs;
using epic8.EventClasses;
using epic8.Field;
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

        //For unique damage multipliers
        public Func<Character, Character, float> UniqueDamageModFormula { get; }

        //For unique flat damage modifiers

        public Func<Character, Character, float> UniqueFlatModFormula { get; }

        //Damage multipliers
        public float AtkRate;
        public float HpScaling;
        public float DefScaling;
        public float Power;
        //This one is for skill enhancements
        public float SkillUps;


        public DamageEffect(float atkRate, float hpScaling, float defScaling, float power, float skillUps,
            Func<Character, Character, float> uniqueDamageMod, Func<Character, Character, float> uniqueFlatDamageMod, EffectTargetType targetType)
        {
            AtkRate = atkRate;
            HpScaling = hpScaling;
            DefScaling = defScaling;
            Power = power;
            SkillUps = skillUps;
            UniqueDamageModFormula = uniqueDamageMod;
            UniqueFlatModFormula = uniqueFlatDamageMod;
            TargetType = targetType;
        }

        public void ApplyEffect(SkillContext skillContext)
        {
            //item1 = damage, item2 = hitType
            foreach (Character target in skillContext.GetTargets(TargetType))
            {
                //Grab how much damage we did to this target, and what kind of hit we made
                Tuple<float, HitType> tuple = DamageCalc.CalculateDamage(skillContext.User, target, this);
                if (tuple.Item2 == HitType.Miss)
                    Console.WriteLine($"{skillContext.User.Name} has missed on {target.Name}!");
                if (tuple.Item2 == HitType.Critical)
                    Console.WriteLine($"{skillContext.User.Name} scores a critical hit on {target.Name}!");
                if (tuple.Item2 == HitType.Crushing)
                    Console.WriteLine($"{skillContext.User.Name} scores a crushing hit on {target.Name}!");

                BattleEvents.PublishAttackResult(new AttackResultEvent(skillContext.User, target, tuple.Item2));
                target.TakeDamage(tuple.Item1);
            }

        }
    }
}
