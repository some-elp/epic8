using epic8.Calcs;
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
    public record OnAttackResult(Character Attacker, Character Target, HitType Hit);
    public record OnTurnEnd(Character Unit);
    public static class BattleEvents
    {
        //List of events
        public static event Action<OnBattleStart>? OnBattleStart;
        public static event Action<OnAttackResult>? OnAttackResult;
        public static event Action<OnTurnEnd>? OnTurnEnd;

        //List of event publishers

        //Start of Battle
        public static void PublishBattleStart(OnBattleStart e)
        {
            OnBattleStart?.Invoke(e);
        }
        //Attack hit/miss
        public static void PublishAttackResult(OnAttackResult e)
        {
            OnAttackResult?.Invoke(e);
        }
        //Turn ended
        public static void PublishTurnEnd(OnTurnEnd e)
        {
            OnTurnEnd?.Invoke(e);
        }
    }
}
