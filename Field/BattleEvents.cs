using epic8.EventClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Field
{
    public static class BattleEvents
    {
        //List of events
        public static event Action<AttackResultEvent>? OnAttackResult;

        //List of event publishers
        public static void PublishAttackResult(AttackResultEvent e)
        {
            OnAttackResult?.Invoke(e);
        }
    }
}
