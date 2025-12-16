using epic8.BuffsDebuffs;
using epic8.Effects;
using epic8.Field;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.PassiveSkills
{
    public class DesignerLilibetS2Passive : Passive
    {
        //subscribe to relevant events
        public override void Initialize()
        {
            BattleEvents.OnBattleStart += HandleBattleStart;
            BattleEvents.OnTurnEnd += HandleTurnEnd;
        }

        //unsubscribe from relevant events
        public override void Dispose()
        {
            BattleEvents.OnBattleStart -= HandleBattleStart;
            BattleEvents.OnTurnEnd -= HandleTurnEnd;
        }

        //Gain 50 fighting spirit at start of battle
        private void HandleBattleStart(OnBattleStart e)
        {
            GainLoseFightingSpirit startBattleFS = new GainLoseFightingSpirit(50f, EffectTargetType.Self);
            EffectContext ctx = new EffectContext(source: Owner, context: BattleContext, passiveSource: this);
            startBattleFS.ApplyEffect(ctx);
        }

        /* Gain 10 fighting spirit for each debuff on an ally at end of enemy's turn
         * When fighting spirit is full, consume all fighting spirit to:
         * - Dispel all debuffs from the caster, grant 1 turn of defense and immunity
         * - Increase caster's CR by 40%
         */
        private void HandleTurnEnd(OnTurnEnd e)
        {
            if(BattleContext.getEnemiesOf(Owner).Contains(e.Unit))
            {
                int count = 0;
                foreach (Character ally in BattleContext.getAlliesOf(Owner))
                {
                    count += ally.StatusEffects.Count(s => !(s.IsBuff));
                }

                if(count > 0)
                {
                    float fsGain = 10 * count;
                    GainLoseFightingSpirit endOfTurnGain = new GainLoseFightingSpirit(fsGain, EffectTargetType.Self);
                    EffectContext ctx = new EffectContext(source: Owner, context: BattleContext, passiveSource: this);
                    endOfTurnGain.ApplyEffect(ctx);
                }
                if (Owner.FightingSpirit >= 100.0f)
                {
                    ActivateMaxFSPassive();
                }
            }
        }

        private void ActivateMaxFSPassive()
        {
            Console.WriteLine($"{Owner.Name}'s Fighting Spirit is full.");
            Owner.FightingSpirit = 0;

            //Full cleanse on self
            CleanseAllDebuffsEffect fullCleanse = new CleanseAllDebuffsEffect(EffectTargetType.Self);
            EffectContext ctx = new EffectContext(source: Owner, context: BattleContext, passiveSource: this);
            fullCleanse.ApplyEffect(ctx);

            //1 turn immunity for self
            ApplyImmunity applyImmunity = new ApplyImmunity(1, EffectTargetType.Self);
            applyImmunity.ApplyEffect(ctx);

            //1 turn Immunity buff
            Owner.AddStatusEffect(new Immunity(1, Owner));

            //1 turn defense buff
            StatChange defenseBuff = new StatChange("Increase Defense", 1, "Increase Defense", true, TickTime.StartOfTurn,
                [new StatModifier(StatType.Defense, 0.6f, 0f)]);
            ApplyStatChangeEffect defBuff = new ApplyStatChangeEffect(defenseBuff, EffectTargetType.Self);
            defBuff.ApplyEffect(ctx);

            //40% cr push
            CRPushEffect crPush = new CRPushEffect(0.4f, EffectTargetType.Self);
            crPush.ApplyEffect(ctx);
        }
    }
}
