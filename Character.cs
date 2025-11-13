using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using epic8.Calcs;
using epic8.Skills;

namespace epic8
{
    public class Character
    {
        public string Name { get; }
        public Element Element { get; }
        public string Role { get; }

        public Stats BaseStats { get; }
        public Stats CurrentStats { get; }

        public List<Skill> Skills { get; }

        public float CurrentHP { get; set; }
        public float CRMeter { get; set; } //number between 0 and 1
        public bool isAlive { get; set; } = true;

        public Character(string name, Element element, string role, Stats baseStats, Stats currentStats, List<Skill> skills)
        {
            this.Name = name;
            this.Element = element;
            this.Role = role;
            this.BaseStats = baseStats;
            this.CurrentStats = currentStats;
            this.Skills = skills;
            this.CurrentHP = currentStats.Hp;
        }

        public void takeTurn(List<Character> enemies)
        {
            Skill skill = this.Skills.First();
            Character target = enemies.FirstOrDefault(e => e.isAlive);
            if (target == null)
            {
                return;
            }
            skill.UseSkill(this, target);
        }

        public override string ToString()
        {
            return $"{Name} (HP, {CurrentStats.Hp}, Speed {BaseStats.Speed}, CR {CRMeter})";
        }


    }
}
