using epic8.BuffsDebuffs;
using epic8.Calcs;
using epic8.Field;
using epic8.NPCBehavior;
using epic8.PassiveSkills;
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

            //Stats objects for each character in this test
            var BaskStats = new Stats(1500f, 1800f, 25000f, 210f, 15f, 150f, 0f, 0f, 0f);
            var AitherStats = new Stats(1500f, 1500f, 15000f, 200f, 15f, 150f, 0f, 0f, 0f);
            var ElsonStats = new Stats(2000f, 1300f, 13000f, 180f, 100f, 250f, 0f, 0f, 0f);
            var TieriaStats = new Stats(2500f, 1000f, 10000f, 190f, 100f, 250f, 0f, 0f, 0f);

            Character Bask = new Character("Bask", Element.Ice, "Knight", BaskStats, [], ControlType.NPC, new BasicNPC());
            Character Aither = new Character("Aither", Element.Ice, "Soulweaver", AitherStats, [], ControlType.NPC, new HealerNPC(0.8f, 0.6f));
            Character Elson = new Character("Elson", Element.Light, "Soulweaver", ElsonStats, [], ControlType.NPC, new BasicNPC());
            Character Tieria = new Character("Tieria", Element.Fire, "Warrior", TieriaStats, [], ControlType.NPC, new BasicNPC());

            //Bask S1, S2, and S3
            Bask.Skills.Add(new Skill("Protective Strike", "blah", 1, TargetType.SingleEnemy,
                [new DamageEffect(0.7f, 0.07f, 0f, 0.9f, 1.4f, (u,t) => 1.0f, (u,t) => 0f, EffectTargetType.SkillTarget)]));
            Bask.Skills.Add(new Skill("Shield Blow", "ble", 3, TargetType.SingleEnemy,
                [new DamageEffect(0.8f, 0.12f, 0f, 1f, 1.3f, (u, t) => 1.0f, (u, t) => 0f, EffectTargetType.SkillTarget),
                new DispelAllBuffsEffect(EffectTargetType.SkillTarget)]));
            Bask.Skills.Add(new Skill("Will of Protection", "b", 5, TargetType.Self,
                [new ApplyImmunity(3, EffectTargetType.AllAllies),
                new ApplyBarrier((ctx, t) => ctx.User.GetEffectiveStats().Hp * 0.3f, 3, EffectTargetType.Self)]));

            //Aither S1, S2, and S3
            Aither.Skills.Add(new Skill("Whispering Spirit", "blah", 1, TargetType.SingleEnemy,
                [new DamageEffect (1f,0f, 0f, 1.05f, 1.25f, (u,t) => 1.0f, (u,t) => 0f, EffectTargetType.SkillTarget),
                new ApplyStatChangeEffect(DecreaseSpeed, EffectTargetType.SkillTarget, 0.5f)]));
            Aither.Skills.Add(new Skill("Guard", "blah", 2, TargetType.SingleAlly,
                [new HealEffect((ctx, t) => t.GetEffectiveStats().Hp * 0.19f * 1.3f, EffectTargetType.SkillTarget),
                new CleanseDebuffEffect(2, EffectTargetType.SkillTarget),
                new CRPushEffect(0.2f, EffectTargetType.Self)]));
            Aither.Skills.Add(new Skill("Spirit's Call", "blah", 3, TargetType.SingleAlly,
                [new HealEffect((ctx, t) => t.GetEffectiveStats().Hp * 0.15f * 1.4f, EffectTargetType.AllAllies),
                new ApplyBarrier((ctx, t) => t.GetEffectiveStats().Hp * 0.15f, 2, EffectTargetType.AllAllies)]));

            //Elson S1, S2, S3
            Elson.Skills.Add(new Skill("Heavy Strike", "blah", 1, TargetType.SingleEnemy,
                [new DamageEffect(0.9f, 0f, 0f, 1f, 1.3f, (u, t) => 1.0f, (u, t) => 0f, EffectTargetType.SkillTarget),
                new ApplyStatChangeEffect(DecreaseAttack, EffectTargetType.SkillTarget, 0.75f)]));
            Elson.Skills.Add(new Skill("Meteor Shower", "blah", 3, TargetType.SingleEnemy,
                [new DamageEffect(0.7f, 0f, 0f, 0.9f, 1.4f, (u, t) => 1.0f, (u,t) => 0f, EffectTargetType.AllEnemies),
                new HealEffect((ctx, t) => t.GetEffectiveStats().Hp * 0.15f * 1.3f, EffectTargetType.AllAllies)]));
            Elson.Skills.Add(new Skill("Light's Protection", "blah", 4, TargetType.SingleAlly,
                [new HealEffect((ctx, t) => t.GetEffectiveStats().Hp * 0.1f, EffectTargetType.AllAllies),
                new ApplyStatChangeEffect(IncreaseAttack, EffectTargetType.AllAllies),
                new ApplyStatChangeEffect(IncreaseDefense, EffectTargetType.AllAllies)]));

            //Tieria S1, S2, and S3
            Tieria.Skills.Add(new Skill("Demon Cutter", "bla", 1, TargetType.SingleEnemy,
                [new DamageEffect(1.0f, 0f, 0f, 1.05f, 1.25f, (u, t) => 1.0f, (u, t) => 0f, EffectTargetType.SkillTarget),
                new ApplyDecreaseHitChance(1, EffectTargetType.SkillTarget, 0.75f)]));
            Tieria.Passives.Add(new TieriaS2Passive());
            Tieria.Skills.Add(new Skill("Soul Break", "blah", 4, TargetType.SingleEnemy,
                [new DamageEffect(1.8f, 0f, 0f, 0.9f, 1.4f, (u, t) => 1.0f, (u, t) => t.GetEffectiveStats().Hp * 0.06f, EffectTargetType.SkillTarget)]));

            //Put the characters into teams
            List<Character> team1 = new List<Character> { Elson, Aither };
            List<Character> team2 = new List<Character> { Bask, Tieria };

            //Create the match
            Battle battle = new Battle(team1, team2);

            //Start the battle
            battle.Start();
        }
    }
}