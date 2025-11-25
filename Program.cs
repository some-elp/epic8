using epic8.BuffsDebuffs;
using epic8.Calcs;
using epic8.Field;
using epic8.NPCBehavior;
using epic8.Skills;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8
{
    class Program
    {
        static void Main(string[] args)
        {
            //2 turn speed debuff
            StatChange DecreaseSpeed = new StatChange(
                "Decrease Speed",
                duration: 2,
                isDebuff: true,
                mods: new StatModifier(StatType.Speed, percentChange: -0.3f));
            //1 turn attack debuff
            StatChange DecreaseAttack = new StatChange(
                "Decrease Attack",
                duration: 1,
                isDebuff: true,
                mods: new StatModifier(StatType.Attack, percentChange: -0.6f));
            //2 turn attack buff
            StatChange IncreaseAttack = new StatChange(
                "Increase Attack",
                duration: 2,
                isBuff: true,
                mods: new StatModifier(StatType.Attack, percentChange: 0.5f));
            //2 turn defense buff
            StatChange IncreaseDefense = new StatChange(
                "Increase Defense",
                duration: 2,
                isBuff: true,
                mods: new StatModifier(StatType.Defense, percentChange: 0.6f));

            var MLAriaStats = new Stats(1500f, 1500f, 30000f, 160f, 100.0f, 250.0f, 0.0f, 0.0f, 0.0f);
            var MLArunkaStats = new Stats(1500f, 3000f, 25000f, 120f, 30f, 150f, 0f, 0f, 0f);
            var AitherStats = new Stats(1500f, 1500f, 15000f, 200f, 15f, 150f, 0f, 0f, 0f);
            var ElsonStats = new Stats(2000f, 1300f, 13000f, 180f, 100f, 250f, 0f, 0f, 0f);

            Character MLAria = new Character("Disciplinary Prefect Aria", Element.Dark, "Warrior", MLAriaStats, [], ControlType.NPC, new BasicNPC());
            Character MLArunka = new Character("Boss Arunka", Element.Light, "Knight", MLArunkaStats, [], ControlType.NPC, new BasicNPC());
            Character Aither = new Character("Aither", Element.Ice, "Soulweaver", AitherStats, [], ControlType.NPC, new HealerNPC(0.8f, 0.6f));
            Character Elson = new Character("Elson", Element.Light, "Soulweaver", ElsonStats, [], ControlType.NPC, new BasicNPC());

            MLAria.Skills.Add(new Skill("Disciplinary Warning", "bleh", 1, 0.5f, 0.05f, 0f, 1f, 1.3f, TargetType.SingleEnemy,
                [new DamageEffect(EffectTargetType.SkillTarget)]));
            MLArunka.Skills.Add(new Skill("Road Sign Smash", "blah", 1, 0.6f, 0f, 1.0f, 1f, 1.3f, TargetType.SingleEnemy,
                [new DamageEffect(EffectTargetType.SkillTarget)]));

            //Aither S1, S2, and S3
            Aither.Skills.Add(new Skill("Whispering Spirit", "blah", 1, 1f,0f, 0f, 1.05f, 1.25f, TargetType.SingleEnemy,
                [new DamageEffect (EffectTargetType.SkillTarget),
                new ApplyStatChangeEffect(DecreaseSpeed, EffectTargetType.SkillTarget, 0.5f)]));
            Aither.Skills.Add(new Skill("Guard", "blah", 2, 0f, 0f, 0f, 0f, 0f, TargetType.SingleAlly,
                [new HealEffect((ctx, t) => t.GetEffectiveStats().Hp * 0.19f * 1.3f, EffectTargetType.SkillTarget),
                new CRPushEffect(0.2f, EffectTargetType.Self)]));
            Aither.Skills.Add(new Skill("Spirit's Call", "blah", 3, 0f, 0f, 0f, 0f, 0f, TargetType.SingleAlly,
                [new HealEffect((ctx, t) => t.GetEffectiveStats().Hp * 0.15f * 1.4f, EffectTargetType.AllAllies),
                new ApplyBarrier((ctx, t) => t.GetEffectiveStats().Hp * 0.15f, 2, EffectTargetType.AllAllies)]));

            //Start here tomorrow with Elson skills, need to test, also change NPCs to handle ally targeting.
            Elson.Skills.Add(new Skill("Heavy Strike", "blah", 1, 0.9f, 0f, 0f, 1f, 1.3f, TargetType.SingleEnemy,
                [new DamageEffect(EffectTargetType.SkillTarget),
                new ApplyStatChangeEffect(DecreaseAttack, EffectTargetType.SkillTarget, 0.75f)]));
            Elson.Skills.Add(new Skill("Meteor Shower", "blah", 3, 0.7f, 0f, 0f, 0.9f, 1.4f, TargetType.SingleEnemy,
                [new DamageEffect(EffectTargetType.AllEnemies),
                new HealEffect((ctx, t) => t.GetEffectiveStats().Hp * 0.15f * 1.3f, EffectTargetType.AllAllies)]));
            Elson.Skills.Add(new Skill("Light's Protection", "blah", 4, 0f, 0f, 0f, 0f, 0f, TargetType.SingleAlly,
                [new HealEffect((ctx, t) => t.GetEffectiveStats().Hp * 0.1f, EffectTargetType.AllAllies),
                new ApplyStatChangeEffect(IncreaseAttack, EffectTargetType.AllAllies),
                new ApplyStatChangeEffect(IncreaseDefense, EffectTargetType.AllAllies)]));

            List<Character> team1 = new List<Character> { Elson, Aither };
            List<Character> team2 = new List<Character> { MLArunka };

            Battle battle = new Battle(team1, team2);

            battle.Start();
        }
    }
}