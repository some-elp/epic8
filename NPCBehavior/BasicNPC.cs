using epic8.BuffsDebuffs;
using epic8.Calcs;
using epic8.Field;
using epic8.Skills;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace epic8.NPCBehavior
{
    public class BasicNPC : INPCController
    {

        /* 
         * 1. ignore invincible targets (not important for now)
         * 2. Attack target where we have elemental advantage
         * 3. Attack lowest %hp target
         * 4. Random target
         * 
         * S3 -> S2 -> S1 skill prio
         * 
         * If there are multiple units that we can hit with elemental advantage
         * Hit the one with lowest %hp.
         * If they are the same hp, select randomly between those targets
         * 
         * Future: 40% chance to hit front target, 20% for rest
         */

        private readonly Random rng = new Random();

        public (Skill, Character target) ChooseAction(Character user, BattleContext context)
        {
            //just pick the first skill
            Skill skill = user.Skills.First();

            //If we have provoke debuff
            if(user.StatusEffects.Any(e => e is Provoke))
            {
                //forced to select basic attack
                skill = user.Skills[0];
                Character target = user.StatusEffects.OfType<Provoke>().FirstOrDefault().AppliedBy;
                return (skill, target);
            }

            //loop through skills in reverse
            for(int i = user.Skills.Count; i > 0; i -= 1)
            {
                //pick the first skill that is off cooldown
                if (user.Skills[i-1].CurrentCooldown == 0)
                {
                    skill = user.Skills[i-1];
                    break;
                }
            }

            if (skill.TargetType == TargetType.SingleEnemy)
            {
                //grab the list of alive enemies
                List<Character> aliveEnemies = context.getEnemiesOf(user).Where(e => e.isAlive).ToList();

                //list of enemies we have elemental advantage against to be populated
                List<Character> eleAdvantage = [];

                //list of enemies that do not have elemental advantage against us
                List<Character> eleNeutral = [];


                //populate list of enemies we have elemental advantage against
                foreach (Character enemy in aliveEnemies)
                {
                    if (ElementHelper.GetElementalAdvantage(user.Element, enemy.Element) == ElementalAdvantage.Advantage)
                    {
                        eleAdvantage.Add(enemy);
                    }
                }

                //populate list of enemies we are elementally neutral against
                foreach (Character enemy in aliveEnemies)
                {
                    if (ElementHelper.GetElementalAdvantage(user.Element, enemy.Element) == ElementalAdvantage.Neutral)
                    {
                        eleNeutral.Add(enemy);
                    }
                }

                if (eleAdvantage.Count > 0)
                {
                    //Select only among enemies we have elemental advantage against
                    return (skill, ChooseRandom(eleAdvantage));
                }
                else if (eleNeutral.Count > 0)
                {
                    //Select among enemies that we don't have disadvantage against
                    return (skill, ChooseRandom(eleNeutral));
                }
                else
                {
                    //Select among all enemies
                    return (skill, ChooseRandom(aliveEnemies));
                }
            }
            else if(skill.TargetType == TargetType.SingleAlly)
            {
                //For now, just pick any random alive ally
                List<Character> aliveAllies = context.getAlliesOf(user).Where(c => c.isAlive).ToList();
                return (skill, aliveAllies[rng.Next(aliveAllies.Count)]);
            }
            else
            {
                //This should be a skill which has TargetType.Self
                return (skill, user);
            }
        }

        private Character ChooseRandom(List<Character> options)
        {
            //Find lowest %hp target
            float lowestHpPercent = options.Min(c => c.CurrentHP / c.GetEffectiveStats().Hp);
            List<Character> targets = [];

            foreach (Character character in options)
            {
                //Add all lowest %hp targets to the list
                if(character.CurrentHP / character.GetEffectiveStats().Hp == lowestHpPercent)
                {
                    targets.Add(character);
                }
            }

            //randomly return a lowest hp target
            return targets[rng.Next(targets.Count)];
        }
    }
}
