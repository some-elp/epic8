using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using epic8.Calcs;
using epic8.NPCBehavior;
using epic8.Skills;

namespace epic8
{

    public enum ControlType { Player, NPC }
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

        public ControlType Control { get; }
        public INPCController? NPCController { get; }

        public Character(string name, Element element, string role, Stats baseStats, Stats currentStats, List<Skill> skills, ControlType control, INPCController? npc = null)
        {
            Name = name;
            Element = element;
            Role = role;
            BaseStats = baseStats;
            CurrentStats = currentStats;
            Skills = skills;
            CurrentHP = currentStats.Hp;
            Control = control;
            NPCController = npc;
        }

        public void takeTurn(List<Character> enemies, List<Character> allies)
        {
            //Determine whether the player gets to control this unit or not.
            if(Control == ControlType.NPC)
            {
                NPCTurn(enemies, allies);
            }
            else
            {
                PlayerTurn(enemies, allies);
            }
        }

        private void NPCTurn(List<Character> enemies, List<Character> allies)
        {
            if (NPCController == null)
            {
                Console.WriteLine($"{Name} has no AI behavior, skipping turn");
            }
            else
            {
                var (skill, target) = NPCController.ChooseAction(this, allies, enemies);
                if (target == null)
                {
                    return;
                }
                skill.UseSkill(this, target);
            }
        }

        private void PlayerTurn(List<Character> enemies, List<Character> allies)
        {
            //Show skills of the current unit
            for (int i=0; i < this.Skills.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {this.Skills[i].Name}");
            }
            //User picks a skill by pressing 1-3
            int skillChoice;
            while (true)
            {
                Console.Write("Choose a skill number: ");
                string? input = Console.ReadLine();
                //We read it as 0-2 for the list
                if (int.TryParse(input, out skillChoice) && skillChoice >= 1 && skillChoice <= this.Skills.Count)
                {
                    skillChoice -= 1;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Input. Please enter a valid skill number.");
                }
            }

            Skill skill = this.Skills[skillChoice];

            //List only enemies that are alive
            List<Character> aliveEnemies = enemies.Where(e => e.isAlive).ToList();

            if (skill.TargetType == TargetType.SingleEnemy)
            {



                for (int j = 0; j < aliveEnemies.Count; j++)
                {
                    Console.WriteLine($"{j + 1}: {aliveEnemies[j]}");
                }

                int targetChoice;

                //Target selection input validation
                while (true)
                {
                    Console.Write("Choose a target: ");
                    string? input = Console.ReadLine();

                    if (int.TryParse(input, out targetChoice) && targetChoice >= 1 && targetChoice <= aliveEnemies.Count)
                    {
                        //read value as 0-2 for the list
                        targetChoice -= 1;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid target number.");
                    }
                }

                Character target = aliveEnemies[targetChoice];
                skill.UseSkill(this, target);
            }
            else if(skill.TargetType == TargetType.AllEnemies)
            {
                //TODO: change Skill class to allow aoe targeting
            }
            else if(skill.TargetType == TargetType.SingleAlly)
            {
                //TODO: copy enemy targeting but for allies
            }
            else if(skill.TargetType == TargetType.AllAllies)
            {
                //TODO: change Skill class to allow aoe targeting
            }
        }

        public override string ToString()
        {
            return $"{Name} (HP, {CurrentHP}/{CurrentStats.Hp}, Speed {BaseStats.Speed}, CR {CRMeter})";
        }


    }
}
