using epic8.BuffsDebuffs;
using epic8.PassiveSkills;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Field
{
    public class Battle
    {
        private readonly List<Character> _team1;
        private readonly List<Character> _team2;

        private TurnManager _turnManager;

        public Battle(List<Character> team1, List<Character> team2)
        {
            _team1 = team1;
            _team2 = team2;
            _turnManager = new TurnManager(_team1.Concat(_team2).ToList());

        }

        public void Start()
        {
            Console.WriteLine("--- Battle Start! ---");
            //Start of battle

            BattleContext context = new BattleContext(_team1, _team2);
            context.InitializePassives();

            //Trigger start of battle passives
            BattleEvents.PublishBattleStart(new OnBattleStart());

            //Keep looping while both teams have at least 1 unit alive
            while (_team1.Any(c => c.isAlive) && _team2.Any(c => c.isAlive))
            {
                //Get next acting unit
                Character acting = _turnManager.GetNextTurn();
                List<Character> enemies = context.getEnemiesOf(acting);
                List<Character> allies = context.getAlliesOf(acting);

                //Loop again if we somehow picked up a dead unit to take the turn.
                if (!acting.isAlive)
                {
                    continue;
                }

                //Acting unit takes its turn.
                context.ActingUnit = acting;
                acting.takeTurn(context);
                //Check to see if any units have died after this turn.
                foreach (Character unit in _team1.Concat(_team2))
                {
                    if (unit.CurrentHP <= 0 && unit.isAlive)
                    {
                        unit.isAlive = false;
                        foreach (StatusEffect effect in unit.StatusEffects)
                        {
                            //not sure if we need this for debuffs, or at all honestly.
                            effect.OnExpire(unit);

                            //remove the debuff.
                            unit.StatusEffects.Remove(effect);
                        }
                        BattleEvents.PublishOnDefeatResult(new OnDefeat(unit, context));
                        foreach (Passive passive in unit.Passives)
                        {
                            passive.Dispose();
                        }
                    }
                }
                /* check for extra turns. Most likely will have to move to an int for multiple extra turns
                 * as well as a loop to check extra turns for all characters.
                 */
                if (acting.ExtraTurns > 0)
                {
                    acting.ExtraTurns -= 1;

                    _turnManager.PrintTimeline();
                    _turnManager.TurnCount++;
                    Console.WriteLine($"{acting.Name} takes an extra turn.");

                    context.ActingUnit = acting;
                    acting.takeTurn(context);
                }

                //Check to see if any units have died after this turn.
                foreach ( Character unit in _team1.Concat(_team2))
                {
                    if (unit.CurrentHP <= 0 && unit.isAlive)
                    {
                        unit.isAlive = false;
                        foreach (StatusEffect effect in unit.StatusEffects)
                        {
                            //not sure if we need this for debuffs, or at all honestly.
                            effect.OnExpire(unit);

                            //remove the debuff.
                            unit.StatusEffects.Remove(effect);
                        }
                        BattleEvents.PublishOnDefeatResult(new OnDefeat(unit, context));
                        foreach (Passive passive in unit.Passives)
                        {
                            passive.Dispose();
                        }
                    }
                }
            }
            //After one team has been defeated, display the winning team.
            Console.WriteLine(_team1.Any(c => c.isAlive) ? "Team 1 wins." : "Team 2 wins.");
        }
    }
}
