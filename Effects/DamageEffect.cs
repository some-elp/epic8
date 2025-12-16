using epic8.Calcs;
using epic8.Field;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Effects
{
    public class DamageEffect : IEffect
    {
        public EffectTargetType TargetType { get; set; }

        //For unique damage multipliers
        public Func<EffectContext, Character, float> UniqueDamageModFormula { get; }

        //For unique flat damage modifiers

        public Func<EffectContext, Character, float> UniqueFlatModFormula { get; }

        //Damage multipliers
        public float AtkRate;
        public float HpScaling;
        public float DefScaling;
        public float Power;
        //This one is for skill enhancements
        public float SkillUps;

        public float defPen { get; set; } = 0f;


        public DamageEffect(float atkRate, float hpScaling, float defScaling, float power, float skillUps,
            Func<EffectContext, Character, float> uniqueDamageMod, Func<EffectContext, Character, float> uniqueFlatDamageMod, EffectTargetType targetType)
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

        public void ApplyEffect(EffectContext effectContext)
        {
            foreach (var mod in effectContext.Source.EffectModifiers)
            {
                AtkRate += mod.ModifyEffect(this, effectContext);
            }
            //item1 = damage, item2 = hitType
            foreach (Character target in effectContext.GetTargets(TargetType))
            {
                BattleEvents.PublishOnBeforeAttack(new OnBeforeAttack(effectContext));

                //Grab how much damage we did to this target, and what kind of hit we made
                Tuple<float, HitType> tuple = DamageCalc.CalculateDamage(effectContext, target, this);

                effectContext.HitResults[target] = tuple.Item2;
                if (tuple.Item2 == HitType.Miss)
                    Console.WriteLine($"{effectContext.Source.Name} has missed on {target.Name}!");
                if (tuple.Item2 == HitType.Critical)
                    Console.WriteLine($"{effectContext.Source.Name} scores a critical hit on {target.Name}!");
                if (tuple.Item2 == HitType.Crushing)
                    Console.WriteLine($"{effectContext.Source.Name} scores a crushing hit on {target.Name}!");

                target.TakeDamage(tuple.Item1, effectContext.BattleContext);
                BattleEvents.PublishAttackResult(new OnAttackResult(effectContext, target, tuple.Item2));
                if (!target.isAlive)
                {
                    effectContext.DefeatedUnits++;
                }
            }

        }
    }
}
