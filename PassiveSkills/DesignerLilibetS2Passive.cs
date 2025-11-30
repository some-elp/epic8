using epic8.BuffsDebuffs;
using epic8.Field;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.PassiveSkills
{
    public class DesignerLilibetS2Passive : PassiveSkill
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
            Owner.FightingSpirit += 50.0f;
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
                    count += ally.StatusEffects.Count(s => s.IsDebuff);
                }

                if(count > 0)
                {
                    float fsGain = 10 * count;
                    Console.WriteLine($"{Owner.Name} gains {fsGain} Fighting Spirit from debuffs on allies.");
                    Owner.FightingSpirit += fsGain;
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

            //get list of all debuffs on caster
            List<StatusEffect> debuffs = Owner.StatusEffects.Where(e => e.IsDebuff).ToList();
            foreach (StatusEffect debuff in debuffs)
            {
                //not sure if we need this for debuffs, or at all honestly.
                debuff.OnExpire(Owner);

                //remove the debuff.
                Owner.StatusEffects.Remove(debuff);
            }

            Console.WriteLine($"{Owner.Name} has cleansed all debuffs on themselves.");

            //1 turn Immunity buff
            Owner.AddStatusEffect(new Immunity(1, Owner));

            //1 turn defense buff
            StatChange defBuff = new StatChange("Increase Defense", 1, true, false, [new StatModifier(StatType.Defense, 0.6f, 0f)]);
            Owner.AddStatusEffect(defBuff);

            Console.WriteLine($"{Owner.Name} has received Immunity and Increase Defense buffs for 1 turn.");

            //40% cr push
            Owner.CRMeter += 0.4f;

            Console.WriteLine($"{Owner.Name} gains 40% Combat Readiness.");
        }
    }
}
