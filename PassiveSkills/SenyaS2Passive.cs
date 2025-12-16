using epic8.BuffsDebuffs;
using epic8.Calcs;
using epic8.Effects;
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
            if(e.effectContext.Source != Owner)
            {
                return;
            }

            e.effectContext.CanCrush = false;
            e.effectContext.CanCrit = false;
        }

        //Damage when hit by a noncrit and activate Grace of the Battlefield skill.
        private void HandleAttackResult(OnAttackResult e)
        {

            EffectContext ctx = new EffectContext(source: Owner, context: BattleContext, passiveSource: this);
            //won't proc if we didn't get hit
            if(e.Target != Owner)
            {
                return;
            }

            //won't proc if we were crit
            if(e.Hit == HitType.Critical)
            {
                return;
            }

            //skill user takes damage equal to 45% of user's attack,70% defpen
            float effectiveDef = e.effectContext.Source.GetEffectiveStats().Defense * 0.3f;
            float damage = (float)Math.Round(0.45f * Owner.GetEffectiveStats().Attack*1.871/(effectiveDef/300+1));

            Console.WriteLine($"{e.effectContext.Source.Name} takes damage from Senya's passive.");
            e.effectContext.Source.TakeDamage(damage);

            //if grace cd is 0 then activate grace and put it on cd
            if (_graceCooldown <= 0)
            {
                //Put passive proc into the queue
                e.effectContext.BattleContext.EnqueueReaction(() =>
                {
                    _graceCooldown = 2;

                    //grant 2 turn barrier to all allies
                    Console.WriteLine("Senya activates Grace of the Battlefield.");

                    ApplyBarrier teamBarrier = new ApplyBarrier((context, t) => context.Source.GetEffectiveStats().Attack * 0.25f, 2, EffectTargetType.AllAllies);
                    teamBarrier.ApplyEffect(ctx);

                    //2 turn speed buff for self
                    StatChange spdBuff = new StatChange("Increase Speed", 2, "Increase Speed", true, TickTime.StartOfTurn,
                    [new StatModifier(StatType.Speed, 0.3f, 0f)]);
                    ApplyStatChangeEffect speedBuff = new ApplyStatChangeEffect(spdBuff, EffectTargetType.Self);
                    speedBuff.ApplyEffect(ctx);
                });
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
