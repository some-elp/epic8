using epic8.Calcs;
using epic8.Effects;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Field
{
    //List of Event Records

    public record OnBattleStart();

    public record OnTurnEnd(Character Unit);
    public record OnBeforeAttack(EffectContext effectContext);

    //Target here should be the effect target.
    public record OnAttackResult(EffectContext effectContext, Character Target, HitType Hit);

    //Unit = unit that has been defeated
    public record OnDefeat(Character Unit, BattleContext Context);

    public static class BattleEvents
    {
        //List of events
        public static event Action<OnBattleStart>? OnBattleStart;
        public static event Action<OnBeforeAttack>? OnBeforeAttack;
        public static event Action<OnAttackResult>? OnAttackResult;
        public static event Action<OnDefeat>? OnDefeat;
        public static event Action<OnTurnEnd>? OnTurnEnd;

        //List of event publishers

        //Start of Battle
        public static void PublishBattleStart(OnBattleStart e)
        {
            OnBattleStart?.Invoke(e);
        }
        //Just before attack hits
        public static void PublishOnBeforeAttack(OnBeforeAttack e)
        {
            OnBeforeAttack?.Invoke(e);
        }
        //Attack hit/miss
        public static void PublishAttackResult(OnAttackResult e)
        {
            OnAttackResult?.Invoke(e);
        }
        //Character defeated
        public static void PublishOnDefeatResult(OnDefeat e)
        {
            OnDefeat?.Invoke(e);
        }
        //Turn ended
        public static void PublishTurnEnd(OnTurnEnd e)
        {
            OnTurnEnd?.Invoke(e);
        }
    }
}
