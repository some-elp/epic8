using epic8.BuffsDebuffs;
using epic8.Calcs;
using epic8.EffectModifiers;
using epic8.Field;
using epic8.Loaders;
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
            //1 turn def break
            StatChange defBreak = new StatChange(
                "Decrease Defense",
                1,
                "Decrease Defense",
                false,
                TickTime.EndOfTurn,
                [new StatModifier(StatType.Defense, -0.7f)]);
            //2 turn GAB
            StatChange greaterAtk = new StatChange(
                "Greater Attack Buff",
                2,
                "Increase Attack",
                true,
                TickTime.EndOfTurn,
                [new StatModifier(StatType.Attack, 0.75f)]);

            //Stats objects for non-JSON characters in this test
            var TieriaStats = new Stats(2500f, 1000f, 10000f, 190f, 100f, 250f, 0f, 0f, 3f);
            var DesignerLilibetStats = new Stats(1500f, 2000f, 17000f, 220f, 100f, 280f, 0f, 0f, 3f);
            var CermiaStats = new Stats(4000f, 1000f, 10000f, 202f, 100f, 300f, 0f, 0f, 3f);
            var SenyaStats = new Stats(5000f, 1300f, 15000f, 182f, 15f, 150f, 0f, 0f, 3f);

            Character Tieria = new Character("Tieria", Element.Fire, "Warrior", TieriaStats, [], ControlType.NPC, new BasicNPC());
            Character DesignerLilibet = new Character("Designer Lilibet", Element.Dark, "Warrior", DesignerLilibetStats, [], ControlType.NPC, new BasicNPC());
            Character Cermia = new Character("Cermia", Element.Fire, "Warrior", CermiaStats, [], ControlType.NPC, new BasicNPC());
            Character Senya = new Character("Senya", Element.Earth, "Knight", SenyaStats, [], ControlType.NPC, new BasicNPC());

            //Tieria S1, S2, and S3
            Tieria.Skills.Add(new Skill("Demon Cutter", "bla", 1, TargetType.SingleEnemy,
                [new DamageEffect(1.0f, 0f, 0f, 1.05f, 1.25f, (u, t) => 1.0f, (u, t) => 0f, EffectTargetType.SkillTarget),
                new ApplyDecreaseHitChance(1, EffectTargetType.SkillTarget, 0.75f)]));
            Tieria.Passives.Add(new TieriaS2Passive());
            Tieria.Skills.Add(new Skill("Soul Break", "blah", 4, TargetType.SingleEnemy,
                [new DamageEffect(1.8f, 0f, 0f, 0.9f, 1.4f, (u, t) => 1.0f, (u, t) => t.GetEffectiveStats().Hp * 0.06f, EffectTargetType.SkillTarget)]));

            //Designer Lilibet S1, S2, and S3
            DesignerLilibet.Skills.Add(new Skill("Slice to Pieces", "bleh", 1, TargetType.SingleEnemy,
                [new DamageEffect(0.6f, 0f, 1f, 1f, 1.3f, (u, t) => 1.0f, (u, t) => 0f, EffectTargetType.SkillTarget),
                new ApplyStatChangeEffect(defBreak, EffectTargetType.SkillTarget, 0.75f),
                new GainLoseFightingSpirit(25f, EffectTargetType.Self)]));
            DesignerLilibet.Passives.Add(new DesignerLilibetS2Passive());
            DesignerLilibet.Skills.Add(new Skill("Model Disqualification", "bleh", 4, TargetType.SingleEnemy,
                [new DecreaseDebuffDurationEffect(2, EffectTargetType.AllAllies),
                new DamageEffect(0.6f, 0f, 1.15f, 1f, 1.3f, (ctx,t) => 1.0f, (ctx, t) => 0f, EffectTargetType.AllEnemies){defPen = 0.5f},
                new IncreaseSkillCooldownEffect(1, EffectTargetType.AllEnemies),
                new CRPushEffect(0.15f, EffectTargetType.AllAllies),
                new GainLoseFightingSpirit(50.0f, EffectTargetType.Self)]));

            //Cermia S1, S2, and S3
            Cermia.Skills.Add(new Skill("Playing with Fire", "blah", 1, TargetType.SingleEnemy,
                [new DamageEffect(1.2f, 0f, 0f, 1f, 1.3f, (ctx,t) => 1f, (ctx, t) => 0f, EffectTargetType.SkillTarget),
                new ApplyUnhealable(1, EffectTargetType.SkillTarget, 0.75f)]));
            Cermia.Skills.Add(new Skill("Hot Streak!", "bleh", 4, TargetType.Self,
                [new ApplyStatChangeEffect(greaterAtk, EffectTargetType.Self),
                new ResetSkillCooldown(EffectTargetType.Self, 2),
                new GainExtraTurnEffect(EffectTargetType.Self)]));
            Cermia.Skills.Add(new Skill("All-In!", "blah", 5, TargetType.SingleEnemy,
                [new DamageEffect(1.15f, 0f, 0f, 0.9f, 1.4f, (ctx, t) => 1.0f, (ctx, t) => 0f, EffectTargetType.SkillTarget) { defPen = 0.5f}]));

            //Senya S1, S2, and S3, along with the provoke modifier
            Senya.Skills.Add(new Skill("Spear of Vengeance", "a", 1, TargetType.SingleEnemy,
                [new DamageEffect(0.95f, 0f, 0f, 1f, 1.3f, (ctx, t) => 1f, (ctx, t) => 0f, EffectTargetType.SkillTarget),
                new ApplyProvoke(1, EffectTargetType.SkillTarget, 0.75f)]));
            Senya.Passives.Add(new SenyaS2Passive());
            Senya.Skills.Add(new Skill("Dragon Slayer's Strike", "b", 4, TargetType.SingleEnemy,
                [new DamageEffect(1.5f, 0f, 0f, 1.1f, 1.2f, (ctx, t) => 1.0f, (ctx, t) => 0f, EffectTargetType.AllEnemies),
                new ApplyDecreaseHitChance(2, EffectTargetType.AllEnemies),
                new ApplyProvoke(1, EffectTargetType.AllEnemies, 0.85f),
                new ApplyCounterattackBuff(EffectTargetType.Self, 3)]));
            Senya.EffectChanceModifiers.Add(new SenyaS1ProvokeModifier());

            //CharacterLoader test
            Character Aither = CharacterLoader.LoadFromFile("UnitJSONS/Aither.json");
            Character Bask = CharacterLoader.LoadFromFile("UnitJSONS/Bask.json");
            Character Elson = CharacterLoader.LoadFromFile("UnitJSONS/Elson.json");


            //Put the characters into teams
            List<Character> team1 = new List<Character> { Bask, Elson };
            List<Character> team2 = new List<Character> { Aither, Senya };

            //Create the match
            Battle battle = new Battle(team1, team2);

            //Start the battle
            battle.Start();
        }
    }
}