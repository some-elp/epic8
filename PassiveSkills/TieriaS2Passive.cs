using epic8.Units;
using epic8.Field;
using epic8.Calcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using epic8.Effects;

namespace epic8.PassiveSkills
{
    public class TieriaS2Passive : Passive
    {
        private readonly CRPushEffect _crPush = new CRPushEffect(0.15f, EffectTargetType.AlliesExceptSelf);

        //subscribe to relevant events
        public override void Initialize()
        {
            BattleEvents.OnAttackResult += HandleAttack;
        }

        //unsubscribe from relevant events
        public override void Dispose()
        {
            BattleEvents.OnAttackResult -= HandleAttack;
        }

        private void HandleAttack(OnAttackResult e)
        {
            if(e.effectContext.Source != Owner || e.Hit == HitType.Miss)
            {
                return;
            }

            Console.WriteLine($"{Owner.Name}'s passive activates.");

            EffectContext ctx = new EffectContext(source: Owner, context: BattleContext, passiveSource: this);

            _crPush.ApplyEffect(ctx);
        }
    }
}
