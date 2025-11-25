using epic8.BuffsDebuffs;
using epic8.Calcs;
using epic8.NPCBehavior;
using epic8.Skills;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Units
{

    public enum ControlType { Player, NPC }
    public class Character
    {
        public string Name { get; }
        public Element Element { get; }
        public string Role { get; }

        /* Stats that you would see on the stat screen of the character
         * Stats during the battle are represented by Character.GetEffeciveStats()
         */
        public Stats BaseStats { get; }

        //List of 3 skills that this character can use
        public List<Skill> Skills { get; }

        //maybe get rid of this? unsure. 
        public float CurrentHP { get; set; }

        //where are we on the CR bar?
        public float CRMeter { get; set; } //number between 0 and 1

        //is this character still alive
        public bool isAlive { get; set; } = true;

        //Player vs NPC controlled
        public ControlType Control { get; }
        //For holding the type of NPC rules that this character will use.
        public INPCController? NPCController { get; }

        //List of buffs/debuffs affecting this character
        public List<IStatusEffect> StatusEffects { get; } = [];

        public Character(string name, Element element, string role, Stats baseStats, List<Skill> skills, ControlType control, INPCController? npc = null)
        {
            Name = name;
            Element = element;
            Role = role;
            BaseStats = baseStats;
            Skills = skills;
            CurrentHP = baseStats.Hp;
            Control = control;
            NPCController = npc;
        }

        public void AddStatusEffect(IStatusEffect effect)
        {

            if (effect is StatChange sc)
            {
                StatusEffects.RemoveAll(e => e is StatChange other && other.Name == sc.Name);
            }

            effect.OnApply(this);
            StatusEffects.Add(effect);
        }

        public void RemoveExpiredEffects()
        {
            for (int i = StatusEffects.Count - 1; i >= 0; i--)
            {
                if (StatusEffects[i].Duration <= 0)
                {
                    StatusEffects[i].OnExpire(this);
                    StatusEffects.RemoveAt(i);
                }
            }
        }

        public Stats GetEffectiveStats()
        {
            Stats final = BaseStats.Clone();

            //List of stat modifiers:
            float attackPercent = 0f;
            float defensePercent = 0f;
            float hpPercent = 0f;
            float spdPercent = 0f;
            float critPercent = 0f;
            float critDamagePercent = 0f;
            float effectivenessPercent = 0f;
            float effectResistancePercent = 0f;
            float dualAttackChancePercent = 0f;

            foreach (var eff in StatusEffects)
            {
                foreach (var mod in eff.GetStatModifiers())
                {
                    if(mod.Stat == StatType.Attack)
                    {
                        attackPercent += mod.PercentChange;
                    }
                    else if(mod.Stat == StatType.Defense)
                    {
                        defensePercent += mod.PercentChange;
                    }
                    else if(mod.Stat == StatType.HP)
                    {
                        hpPercent += mod.PercentChange;
                    }
                    else if(mod.Stat == StatType.Speed)
                    {
                        spdPercent += mod.PercentChange;
                    }
                    else if(mod.Stat == StatType.CritChance)
                    {
                        critPercent += mod.PercentChange;
                    }
                    else if(mod.Stat == StatType.CritDamage)
                    {
                        critDamagePercent += mod.PercentChange;
                    }
                    else if(mod.Stat == StatType.Effectiveness)
                    {
                        effectivenessPercent += mod.PercentChange;
                    }
                    else if(mod.Stat == StatType.EffectResistance)
                    {
                        effectResistancePercent += mod.PercentChange;
                    }
                    else if(mod.Stat == StatType.DualAttackChance)
                    {
                        dualAttackChancePercent += mod.PercentChange;
                    }
                    else
                    {
                        Console.WriteLine("Stat not identifiable.");
                    }
                }

            }

            final.Attack += (float)(Math.Round(BaseStats.Attack * attackPercent));
            final.Defense += (float)(Math.Round(BaseStats.Defense * defensePercent));
            final.Hp += (float)(Math.Round(BaseStats.Hp * hpPercent));
            final.Speed += (float)(Math.Round(BaseStats.Speed * spdPercent));
            final.CritChance += critPercent;
            final.CritDamage += critDamagePercent;
            final.Effectiveness += effectivenessPercent;
            final.EffectResistance += effectResistancePercent;
            final.DualAttackChance += dualAttackChancePercent;

            return final;
        }

        public void TakeDamage(float amount)
        {
            //find barrier in the buff list
            foreach (BarrierBuff barrier in StatusEffects.Where(b => b is BarrierBuff).ToList())
            {
                //how much damage was absorbed by the barrier
                float absorbed = barrier.AbsorbDamage(amount);
                Console.WriteLine($"{this.Name}'s Barrier takes {absorbed} damage.");

                amount -= absorbed;

                //remove barrier from buff list if the amount of damage was greater than the barrier
                if (barrier.Remaining <= 0)
                {
                    barrier.OnExpire(this);
                    StatusEffects.Remove(barrier);
                    Console.WriteLine($"{this.Name}'s Barrier was broken.");
                }

                //exit method if barrier takes all the damage.
                if (amount <= 0)
                {
                    return;
                }
            }

            CurrentHP -= amount;
            Console.WriteLine($"{Name} has taken {amount} damage.");
            if( CurrentHP <= 0 )
            {
                isAlive = false;
                Console.WriteLine($"{Name} was defeated.");
            }
            else
            {
                Console.WriteLine($"{Name} has {CurrentHP} HP remaining.");
            }

        }

        public void takeTurn(List<Character> allies, List<Character> enemies)
        {
            //Determine whether the player gets to control this unit or not.
            if(Control == ControlType.NPC)
            {
                NPCTurn(allies, enemies);
            }
            else
            {
                PlayerTurn(allies, enemies);
            }

            //Reduce the cooldown of all skills by 1 at the end of turn
            foreach (Skill skill in Skills)
            {
                if(skill.CurrentCooldown > 0)
                {
                    skill.CurrentCooldown -= 1;
                }
            }

            //Reduce duration of buffs and debuffs
            foreach (var statusEffect in StatusEffects)
            {
                statusEffect.Duration -= 1;
            }

            RemoveExpiredEffects();
        }

        private void NPCTurn(List<Character> allies, List<Character> enemies)
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
                skill.UseSkill(this, target, allies, enemies);
            }
        }

        private void PlayerTurn(List<Character> allies, List<Character> enemies)
        {
            //Show skills of the current unit
            for (int i=0; i < Skills.Count; i++)
            {
                if (Skills[i].CurrentCooldown <= 0)
                {
                    Console.WriteLine($"{i + 1}: {Skills[i].Name}");
                }
                else
                {
                    Console.WriteLine($"{i + 1}: {Skills[i].Name} - Cooldown: {Skills[i].CurrentCooldown} ");
                }
            }
            //User picks a skill by pressing 1-3
            int skillChoice;
            while (true)
            {
                Console.Write("Choose a skill number: ");
                string? input = Console.ReadLine();
                /* We read it as 0-2 for the list, can't select a number too large/small
                 * and cannot select a skill that is on cooldown.
                 */
                if (int.TryParse(input, out skillChoice)
                    && skillChoice >= 1
                    && skillChoice <= Skills.Count
                    && Skills[skillChoice-1].CurrentCooldown == 0)
                {
                    skillChoice -= 1;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Input. Please enter a valid skill number.");
                }
            }

            //The skill we chose to use
            Skill skill = Skills[skillChoice];

            //List only enemies that are alive
            List<Character> aliveEnemies = enemies.Where(e => e.isAlive).ToList();

            if (skill.TargetType == TargetType.SingleEnemy)
            {
                //List out all alive enemies
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

                List<Character> target = [aliveEnemies[targetChoice]];
                skill.UseSkill(this, target, allies, enemies);
            }
            else if(skill.TargetType == TargetType.AllEnemies)
            {
                skill.UseSkill(this, aliveEnemies, allies, enemies);
            }
            else if(skill.TargetType == TargetType.SingleAlly)
            {
                //List out all allies
                for (int j = 0; j < allies.Count; j++)
                {
                    Console.WriteLine($"{j + 1}: {allies[j]}");
                }

                int targetChoice;

                //Target selection input validation
                while (true)
                {
                    Console.Write("Choose a target: ");
                    string? input = Console.ReadLine();

                    if (int.TryParse(input, out targetChoice) && targetChoice >= 1 && targetChoice <= allies.Count)
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

                List<Character> target = [allies[targetChoice]];
                skill.UseSkill(this, target, allies, enemies);
            }
            else if(skill.TargetType == TargetType.AllAllies)
            {
                skill.UseSkill(this, allies, allies, enemies);
            }
            else if (skill.TargetType == TargetType.Self)
            {
                skill.UseSkill(this, [this], allies, enemies);
            }
        }

        public override string ToString()
        {
            return $"{Name} (HP, {CurrentHP}/{GetEffectiveStats().Hp}, Speed {GetEffectiveStats().Speed}, CR {CRMeter*100}%)";
        }
    }
}
