using epic8.BuffsDebuffs;
using epic8.Calcs;
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
            StatChange defBreak = new StatChange("Decrease Defense",
                1,
                false,
                true,
                [new StatModifier(StatType.Defense, -0.7f, 0f)]);

            //Stats objects for non-JSON characters in this test
            var TieriaStats = new Stats(2500f, 1000f, 10000f, 190f, 100f, 250f, 0f, 0f, 3f);
            var DesignerLilibetStats = new Stats(1500f, 2000f, 17000f, 175f, 100f, 280f, 0f, 0f, 3f);

            Character Tieria = new Character("Tieria", Element.Fire, "Warrior", TieriaStats, [], ControlType.NPC, new BasicNPC());
            Character DesignerLilibet = new Character("Designer Lilibet", Element.Dark, "Warrior", DesignerLilibetStats, [], ControlType.NPC, new BasicNPC());

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
                new GainLoseFightingSpirit(50f, EffectTargetType.Self)]));


            //CharacterLoader test
            Character Aither = CharacterLoader.LoadFromFile("UnitJSONS/Aither.json");
            Character Bask = CharacterLoader.LoadFromFile("UnitJSONS/Bask.json");
            Character Elson = CharacterLoader.LoadFromFile("UnitJSONS/Elson.json");

            Console.WriteLine(Bask.BaseStats.ToString());

            //Put the characters into teams
            List<Character> team1 = new List<Character> { Bask, Aither };
            List<Character> team2 = new List<Character> { Elson, DesignerLilibet };

            //Create the match
            Battle battle = new Battle(team1, team2);

            //Start the battle
            battle.Start();
        }
    }
}