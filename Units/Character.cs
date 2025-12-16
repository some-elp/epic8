using epic8.BuffsDebuffs;
using epic8.Calcs;
using epic8.EffectModifiers;
using epic8.Field;
using epic8.NPCBehavior;
using epic8.PassiveSkills;
using epic8.Effects;
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
        public ControlType Control { get; set; }
        //For holding the type of NPC rules that this character will use.
        public INPCController? NPCController { get; }

        //List of buffs/debuffs affecting this character
        public List<StatusEffect> StatusEffects { get; } = [];

        //List of non buff/debuff stat modiifiers (artifact, passive, etc)
        public List<StatModifier> OtherStatModifiers { get; set; } = [];

        //List of passives owned by this character
        public List<Passive> Passives { get; set; } = [];

        //Lists of EffectModifiers owned by this character
        //Maybe move this from Character into the skilleffect classes?
        public List<IEffectModifier> EffectModifiers { get; set; } = [];

        //Does this character have an extra turn to take?
        public int ExtraTurns { get; set; } = 0;

        //How much fighting spirit do we have (Max 100, not useful on every unit)
        public float FightingSpirit { get; set; } = 0;

        //How much focus do we have (Max 5, not useful on every unit)
        public float Focus { get; set; } = 0;

        //Critical Hit Resistance
        public float CriticalHitResistance { get; set; } = 0;

        //Evasion
        public float Evasion { get; set; } = 0;

        //Penetration Resistance
        public float PenetrationResistance { get; set; } = 0;


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

        public void AddStatusEffect(StatusEffect effect)
        {
            //Prevent duplicate status effects
            StatusEffect? existingEffect = StatusEffects.FirstOrDefault(s => s.Category == effect.Category);

            //If we don't already have effect of this type
            if (existingEffect == null)
            {
                StatusEffects.Add(effect);
                effect.OnApply(this);
                return;
            }

            //If we do have an effect of this type, but the new one is better (ex: GAB vs regular attack buff)
            if(effect.Priority > existingEffect.Priority)
            {
                //keep the highest duration
                effect.Duration = Math.Max(existingEffect.Duration, effect.Duration);
                StatusEffects.Remove(existingEffect);
                //Add the new effect to the list
                StatusEffects.Add(effect);
                effect.OnApply(this);
            }
            //new effect is the same prio as new one (most likely exact same buff)
            else if (effect.Priority == existingEffect.Priority)
            {
                if (effect.Duration >= existingEffect.Duration)
                {
                    //If the new buff's duration is higher we add a new buff
                    StatusEffects.Remove(existingEffect);
                    StatusEffects.Add(effect);
                    effect.OnApply(this);
                }
                else
                {
                    //Otherwise, we do nothing
                    return;
                }
            }
            //new buff prio is lower (ie: attack buff when we already have GAB)
            else
            {

                if (effect.Duration >= existingEffect.Duration)
                {
                    existingEffect.Duration = effect.Duration;
                    existingEffect.AppliedThisTurn = true;
                }
                else
                {
                    return;
                }
            }

        }

        public void RemoveExpiredEffects()
        {
            //Remove all effects that are 0 duration
            for (int i = StatusEffects.Count - 1; i >= 0; i--)
            {
                if (StatusEffects[i].Duration <= 0)
                {
                    StatusEffects[i].OnExpire(this);
                    StatusEffects.RemoveAt(i);
                }
            }
        }

        public bool IsImmune()
        {
            //for now, unaffected by negative effects if we have immunity buff
            return StatusEffects.Any(e => e is Immunity);
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

            //Go through our list of statuseffects, and get the total stat changes (additive)
            foreach (StatusEffect eff in StatusEffects)
            {
                foreach (StatModifier mod in eff.GetStatModifiers())
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

            foreach(StatModifier mod in OtherStatModifiers)
            {
                if (mod.Stat == StatType.Attack)
                {
                    attackPercent += mod.PercentChange;
                }
                else if (mod.Stat == StatType.Defense)
                {
                    defensePercent += mod.PercentChange;
                }
                else if (mod.Stat == StatType.HP)
                {
                    hpPercent += mod.PercentChange;
                }
                else if (mod.Stat == StatType.Speed)
                {
                    spdPercent += mod.PercentChange;
                }
                else if (mod.Stat == StatType.CritChance)
                {
                    critPercent += mod.PercentChange;
                }
                else if (mod.Stat == StatType.CritDamage)
                {
                    critDamagePercent += mod.PercentChange;
                }
                else if (mod.Stat == StatType.Effectiveness)
                {
                    effectivenessPercent += mod.PercentChange;
                }
                else if (mod.Stat == StatType.EffectResistance)
                {
                    effectResistancePercent += mod.PercentChange;
                }
                else if (mod.Stat == StatType.DualAttackChance)
                {
                    dualAttackChancePercent += mod.PercentChange;
                }
                else
                {
                    Console.WriteLine("Stat not identifiable.");
                }
            }

            //Apply our stat changes
            final.Attack += (float)(Math.Round(BaseStats.Attack * attackPercent));
            final.Defense += (float)(Math.Round(BaseStats.Defense * defensePercent));
            final.Hp += (float)(Math.Round(BaseStats.Hp * hpPercent));
            final.Speed += (float)(Math.Round(BaseStats.Speed * spdPercent));
            final.CritChance += critPercent;
            final.CritDamage += critDamagePercent;
            final.Effectiveness += effectivenessPercent;
            final.EffectResistance += effectResistancePercent;
            final.DualAttackChance += dualAttackChancePercent;

            //Return our current stats.
            return final;
        }

        public void TakeDamage(float amount, BattleContext battleContext)
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

            //Take all damage not absorbed by barrier.
            CurrentHP -= amount;
            Console.WriteLine($"{Name} has taken {amount} damage.");
            
            //Character is defeated if we have 0 or less hp
            if( CurrentHP <= 0 )
            {
                isAlive = false;
                foreach (StatusEffect effect in StatusEffects.ToList())
                {
                    //not sure if we need this for debuffs, or at all honestly.
                    effect.OnExpire(this);

                    //remove the debuff.
                    this.StatusEffects.Remove(effect);
                }
                BattleEvents.PublishOnDefeatResult(new OnDefeat(this, battleContext));

                foreach (Passive passive in Passives)
                {
                    passive.Dispose();
                }
                Console.WriteLine($"{Name} was defeated.");
            }
            else
            {
                Console.WriteLine($"{Name} has {CurrentHP} HP remaining.");
            }

        }

        public void takeTurn(BattleContext context)
        {
            //Check start of turn ticking buffs/debuffs
            /* (var statusEffect in StatusEffects.Where(s => s.TickTime == TickTime.StartOfTurn))
            {
                if (statusEffect.AppliedThisTurn)
                {
                    statusEffect.AppliedThisTurn = false;
                    continue;
                }

                statusEffect.Duration -= 1;
            }
            RemoveExpiredEffects();*/

            //Determine whether the player gets to control this unit or not.
            if (Control == ControlType.NPC)
            {
                NPCTurn(context);
            }
            else
            {
                PlayerTurn(context);
            }


            //Trigger on turn end effects.
            BattleEvents.PublishTurnEnd(new OnTurnEnd(this));

            //Reduce the cooldown of all skills by 1 at the end of turn
            foreach (Skill skill in Skills)
            {
                if(skill.CurrentCooldown > 0)
                {
                    skill.CurrentCooldown -= 1;
                }
            }

            //Check end of turn buffs/debuffs
            foreach (var statusEffect in StatusEffects)
            {
                if(statusEffect.AppliedThisTurn)
                {
                    statusEffect.AppliedThisTurn = false;
                    continue;
                }

                statusEffect.Duration -= 1;
            }
            RemoveExpiredEffects();

            //mark appliedthisturn false for all buffs/debuffs
            foreach (Character character in context.getAllCharacters())
            {
                foreach (StatusEffect statusEffect in character.StatusEffects)
                {
                    statusEffect.AppliedThisTurn = false;
                }
            }
        }

        private void NPCTurn(BattleContext context)
        {
            //Shouldn't happen, All Characters should have their behavior specified
            if (NPCController == null)
            {
                Console.WriteLine($"{Name} has no AI behavior, skipping turn");
            }
            else
            {
                //Determine what skill and target the NPC is choosing
                var (skill, target) = NPCController.ChooseAction(this, context);
                if (target == null)
                {
                    return;
                }
                skill.UseSkill(this, target, context);
                context.ResolveReactions();
            }
        }

        private void PlayerTurn(BattleContext context)
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
            //Source picks a skill by pressing 1-3
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
            List<Character> aliveEnemies = context.getEnemiesOf(this).Where(e => e.isAlive).ToList();
            List<Character> allies = context.getAlliesOf(this);

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

                Character target = aliveEnemies[targetChoice];
                skill.UseSkill(this, target, context);
                context.ResolveReactions();
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

                Character target = allies[targetChoice];
                skill.UseSkill(this, target, context);
                context.ResolveReactions();
            }
            else if (skill.TargetType == TargetType.Self)
            {
                //Don't need the user to pick a target in this case.
                skill.UseSkill(this, this, context);
                context.ResolveReactions();
            }
        }

        public override string ToString()
        {
            return $"CR {CRMeter * 100}%, {Name} (HP: {CurrentHP}/{GetEffectiveStats().Hp}, Speed {GetEffectiveStats().Speed}, Status: {string.Join(", ", StatusEffects.Select(s => s.ToString()))})";
        }
    }
}
