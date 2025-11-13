using epic8.Calcs;
using epic8.Field;
using epic8.Skills;
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
            var stats1 = new Stats(1.0f, 1.0f, 100.0f, 300.0f, 100.0f, 150.0f, 1.0f, 1.0f, 1.0f);
            var stats2 = new Stats(1.0f, 1.0f, 100.0f, 269.0f, 25.0f, 150.0f, 1.0f, 1.0f, 1.0f);
            var stats3 = new Stats(1.0f, 1.0f, 100.0f, 150.0f, 25.0f, 150.0f, 1.0f, 1.0f, 1.0f);
            Character test1 = new Character("Sandbag", Element.Fire, "Warrior", stats1, stats1, []);
            Character test2 = new Character("KingSandbag", Element.Ice, "Warrior", stats2, stats2, []);
            Character test3 = new Character("SlowSandbag", Element.Earth, "Thief", stats3, stats3, []);

            test1.Skills.Add(new BasicSkill("Swing", "bleh", 1, 1, 0, 0, 1));
            test2.Skills.Add(new BasicSkill("Fire", "blah", 1, 1, 0, 0, 1));
            test3.Skills.Add(new BasicSkill("Slap", "buh", 1, 1, 0, 0, 1));

            List<Character> team1 = new List<Character> { test1 };
            List<Character> team2 = new List<Character> { test2, test3 };

            Battle battle = new Battle(team1, team2);

            battle.Start();
        }
    }
}