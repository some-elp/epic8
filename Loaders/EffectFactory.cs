using epic8.BuffsDebuffs;
using epic8.Skills;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Loaders
{
    public static class EffectFactory
    {
        public static ISkillEffect Create(SkillEffectData data)
        {
            string type = data.EffectType;

            switch (type)
            {
                case "Damage":
                    return CreateDamage(data);
                case "ApplyStatChange":
                    return CreateStatChange(data);
                case "Heal":
                    return CreateHeal(data);
                case "Barrier":
                    return CreateBarrier(data);
                case "CleanseDebuff":
                    return CreateCleanseDebuff(data);
                case "CRPush":
                    return CreateCRPush(data);
                case "DispelAllBuffs":
                    return CreateDispelAllBuffs(data);
                case "ApplyImmunity":
                    return CreateApplyImmunity(data);
                default:
                    throw new Exception("Unknown Effect Type");

            }
        }
        //DamageEffect
        private static ISkillEffect CreateDamage(SkillEffectData data)
        {
            var d = data.Damage;

            var percentFormula = FormulaParser.Compile(d.PercentFormula);
            var flatFormula = FormulaParser.Compile(d.FlatFormula);

            EffectTargetType TargetType;
            Enum.TryParse(data.EffectTargetType, out TargetType);
            return new DamageEffect(
                atkRate: d.AtkRate,
                hpScaling: d.HpScaling,
                defScaling: d.DefScaling,
                power: d.Power,
                skillUps: d.SkillUps,
                uniqueDamageMod: percentFormula,
                uniqueFlatDamageMod: flatFormula,
                targetType: TargetType);
        }

        //ApplyStatChangeEffect
        private static ISkillEffect CreateStatChange(SkillEffectData data)
        {
            var d = data.StatChange;

            List<StatModifier> modList = [];

            foreach (StatModifierData modData in d.StatModifiers)
            {
                StatType stat;
                Enum.TryParse(modData.Stat, out stat);
                StatModifier mod = new StatModifier(stat, modData.PercentChange, modData.FlatChange);
                modList.Add(mod);
            }


            StatChange statChange = new StatChange(
                d.Name,
                d.Duration,
                d.Category,
                d.IsBuff,
                d.TickTime,
                modList,
                d.IsUndispellable,
                d.Priority);

            EffectTargetType TargetType;
            Enum.TryParse(data.EffectTargetType, out TargetType);
            return new ApplyStatChangeEffect(statChange, TargetType, d.Chance);
        }

        //HealEffect
        private static ISkillEffect CreateHeal(SkillEffectData data)
        {
            var d = data.Heal;

            var formula = FormulaParser.Compile(d.Formula);

            EffectTargetType TargetType;
            Enum.TryParse(data.EffectTargetType, out TargetType);
            return new HealEffect(formula, TargetType);
        }

        //ApplyBarrier
        private static ISkillEffect CreateBarrier(SkillEffectData data)
        {
            var d = data.Barrier;

            var formula = FormulaParser.Compile(d.Formula);

            EffectTargetType TargetType;
            Enum.TryParse(data.EffectTargetType, out TargetType);
            return new ApplyBarrier(formula, d.Duration, TargetType);
        }

        //CleanseDebuff
        private static ISkillEffect CreateCleanseDebuff(SkillEffectData data)
        {
            var d = data.CleanseDebuff;

            EffectTargetType TargetType;
            Enum.TryParse(data.EffectTargetType, out TargetType);
            return new CleanseDebuffEffect(d.Amount, TargetType);
        }

        //CRPush
        private static ISkillEffect CreateCRPush(SkillEffectData data)
        {
            var d = data.CRPush;

            EffectTargetType TargetType;
            Enum.TryParse(data.EffectTargetType, out TargetType);
            return new CRPushEffect(d.Amount, TargetType);
        }

        //DispelAllBuffs
        private static ISkillEffect CreateDispelAllBuffs(SkillEffectData data)
        {
            var d = data.DispelAllBuffs;
            EffectTargetType TargetType;
            Enum.TryParse(data.EffectTargetType, out TargetType);
            return new DispelAllBuffsEffect(TargetType, d.Chance);
        }

        //ApplyImmunity
        private static ISkillEffect CreateApplyImmunity(SkillEffectData data)
        {
            var d = data.ApplyImmunity;
            EffectTargetType TargetType;
            Enum.TryParse(data.EffectTargetType, out TargetType);
            return new ApplyImmunity(d.Duration, TargetType);

        }
    }
}
