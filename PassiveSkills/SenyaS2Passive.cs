using epic8.BuffsDebuffs;
using epic8.Calcs;
using epic8.Field;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace epic8.PassiveSkills
{
    public class SenyaS2Passive : Passive
    {
        private int _graceCooldown = 0;

        //StatModifier attackPassive = new StatModifier(StatType.Attack, 0.3f);
        public override void Initialize()
        {
            //+30% attack passive
            //Owner.OtherStatModifiers.Add(attackPassive);
            //+50% Crit Resist passive
            Owner.CriticalHitResistance += 50f;
            //Senya cannot crit or crushing blow
            BattleEvents.OnBeforeAttack += HandleBeforeAttack;
            //Damage passive and activate Grace of the Battlefield
            BattleEvents.OnAttackResult += HandleAttackResult;
            //Keep track of Grace cooldown
            BattleEvents.OnTurnEnd += HandleTurnEnd;
        }

        public override void Dispose()
        {
            //Owner.OtherStatModifiers.Remove(attackPassive);
            Owner.CriticalHitResistance -= 50f;
            BattleEvents.OnBeforeAttack -= HandleBeforeAttack;
            BattleEvents.OnAttackResult -= HandleAttackResult;
            BattleEvents.OnTurnEnd -= HandleTurnEnd;
        }

        //Cannot crit or crushing blow
        private void HandleBeforeAttack(OnBeforeAttack e)
        {
            if(e.skillContext.User != Owner)
            {
                return;
            }

            e.skillContext.CanCrush = false;
            e.skillContext.CanCrit = false;
        }

        //Damage when hit by a noncrit and activate Grace of the Battlefield skill.
        private void HandleAttackResult(OnAttackResult e)
        {
            if(e.target != Owner)
            {
                return;
            }

            if(e.Hit == HitType.Critical)
            {
                return;
            }

            //skill user takes damage equal to 45% of user's attack,70% defpen
            float effectiveDef = e.skillContext.User.GetEffectiveStats().Defense * 0.3f;
            float damage = (float)Math.Round(0.45f * Owner.GetEffectiveStats().Attack*1.871/(effectiveDef/300+1));

            Console.WriteLine($"{e.skillContext.User.Name} takes damage from Senya's passive.");
            e.skillContext.User.TakeDamage(damage);

            //if grace cd is 0 then activate grace and put it on cd
            if (_graceCooldown <= 0)
            {
                _graceCooldown = 2;
                //grant 2 turn barrier to all allies
                Console.WriteLine("Senya activates Grace of the Battlefield.");
                foreach(Character character in BattleContext.getAlliesOf(Owner))
                {
                    character.AddStatusEffect(new BarrierBuff(2, Owner.GetEffectiveStats().Attack * 0.25f, Owner));
                    Console.WriteLine($"{character.Name} has received a barrier of {Owner.GetEffectiveStats().Attack * 0.25}");
                }
                
                //2 turn speed buff for self
                StatChange spdBuff = new StatChange("Increase Speed", 2, "Increase Speed", true, TickTime.StartOfTurn,
                [new StatModifier(StatType.Speed, 0.3f, 0f)])
                { AppliedBy = Owner };
                Owner.AddStatusEffect(spdBuff);
                Console.WriteLine($"{Owner.Name} has been affected by {spdBuff.Name} for 2 turns.");
            }
        }

        //reduce grace cooldown by 1 at end of owner's turn.
        private void HandleTurnEnd(OnTurnEnd e)
        {
            if (e.Unit == Owner && _graceCooldown > 0)
            {
                _graceCooldown -= 1;
            }
        }
    }
}
