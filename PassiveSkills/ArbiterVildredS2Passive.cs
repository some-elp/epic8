using epic8.Effects;
using epic8.Field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.PassiveSkills
{
    public class ArbiterVildredS2Passive : Passive
    {
        private int _darkContractCooldown = 0;

        public override void Initialize()
        {
            BattleEvents.OnDefeat += HandleDefeat;
            BattleEvents.OnTurnEnd += HandleTurnEnd;
        }

        public override void Dispose()
        {
            BattleEvents.OnDefeat -= HandleDefeat;
            BattleEvents.OnTurnEnd -= HandleTurnEnd;
        }

        //Revive to 100% hp, gain 100% CR, gain max focus (5) reset skill 3 cooldown
        private void HandleDefeat(OnDefeat e)
        {
            if (e.Unit != Owner)
            {
                return;
            }

            if(_darkContractCooldown <= 0)
            {
                e.Context.EnqueueReaction(() =>
                {
                    _darkContractCooldown = 5;
                    EffectContext ctx = new EffectContext(source: Owner, e.Context, passiveSource: this);
                    Console.WriteLine("Arbiter Vildred's passive activates.");

                    //Revive with 100% hp
                    ReviveEffect revive = new ReviveEffect(1f, EffectTargetType.Self);
                    revive.ApplyEffect(ctx);

                    //Don't do any of these if revive was blocked somehow
                    if (Owner.isAlive)
                    {
                        //CR Push self by 100%
                        CRPushEffect crPush = new CRPushEffect(1f, EffectTargetType.Self);
                        crPush.ApplyEffect(ctx);

                        //Gain max focus (5)
                        GainLoseFocus gainMaxFocus = new GainLoseFocus(5f, EffectTargetType.Self);
                        gainMaxFocus.ApplyEffect(ctx);

                        //Reset cooldown of S3
                        ResetSkillCooldown resetS3 = new ResetSkillCooldown(EffectTargetType.Self, 1);
                        resetS3.ApplyEffect(ctx);
                    }
                });
            }
        }

        //Reduce passive CD by 1 at the end of turn.
        private void HandleTurnEnd(OnTurnEnd e)
        {
            if (e.Unit == Owner && _darkContractCooldown > 0)
            {
                _darkContractCooldown -= 1;
            }
        }
    }
}
