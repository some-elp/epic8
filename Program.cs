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
            //hopefully speed debuff
            StatChange DecreaseSpeed = new StatChange(
                "Decrease Speed",
                duration: 2,
                isDebuff: true,
                mods: new StatModifier(StatType.Speed, percentChange: -0.3f));

            var MLAriaStats = new Stats(1500f, 1500f, 30000f, 160f, 100.0f, 250.0f, 0.0f, 0.0f, 0.0f);
            var MLArunkaStats = new Stats(1500f, 3000f, 25000f, 120f, 30f, 150f, 0f, 0f, 0f);

            Character MLAria = new Character("Disciplinary Prefect Aria", Element.Dark, "Warrior", MLAriaStats, MLAriaStats, [], ControlType.NPC, new BasicNPC());
            Character MLArunka = new Character("Boss Arunka", Element.Light, "Knight", MLArunkaStats, MLArunkaStats, [], ControlType.NPC, new BasicNPC());

            MLAria.Skills.Add(new Skill("Disciplinary Warning", "bleh", 1, 0.5f, 0.05f, 0f, 1f, 1.3f, TargetType.SingleEnemy, [new DamageEffect(), new ApplyStatChangeEffect(DecreaseSpeed, EffectTargetType.SkillTargets, 0.5f)]));
            MLArunka.Skills.Add(new Skill("Road Sign Smash", "blah", 1, 0.6f, 0f, 1.0f, 1f, 1.3f, TargetType.SingleEnemy, [new DamageEffect()]));

            List<Character> team1 = new List<Character> { MLAria };
            List<Character> team2 = new List<Character> { MLArunka };

            Battle battle = new Battle(team1, team2);

            battle.Start();
        }
    }
}