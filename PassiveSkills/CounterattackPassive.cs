using epic8.Field;
using epic8.Effects;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.PassiveSkills
{
    public class CounterattackPassive : Passive
    {
        private float _chance;
        private Random _rng = new Random();

        public override bool CanSeal => false;

        public CounterattackPassive(Character owner, float chance = 1.0f)
        {
            _chance = chance;
            Owner = owner;
        }

        public override void Initialize()
        {
            BattleEvents.OnAttackResult += HandleAttackResult;
        }

        public override void Dispose()
        {
            BattleEvents.OnAttackResult -= HandleAttackResult;
        }

        private void HandleAttackResult(OnAttackResult e)
        {


            //Only counter if we were hit
            if(e.Target != Owner)
            {
                return;
            }

            //don't counter if the skill we got hit by was a counter
            if(e.effectContext.BattleContext.ActingUnit != e.effectContext.Source)
            {
                return;
            }

            //don/t counter if the skill user was defeated by some kind of proc damage
            if (!(e.effectContext.Source.isAlive))
            {
                return;
            }

            //check if we made the counter% chance
            if(_rng.NextDouble() <= _chance)
            {
                //put counter into the queue
                e.effectContext.BattleContext.EnqueueReaction(() =>
                {
                    //Attack effectContext.user with our Skill 1
                    Console.WriteLine($"{Owner.Name} counterattacks.");
                    Owner.Skills[0].UseSkill(Owner, e.effectContext.Source, e.effectContext.BattleContext);
                });
            }
        }
    }
}
