using epic8.Units;
using epic8.Field;
using epic8.Calcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using epic8.EventClasses;

namespace epic8.PassiveSkills
{
    public class TieriaS2Passive : PassiveSkill
    {
        public override void Initialize()
        {
            BattleEvents.OnAttackResult += HandleAttack;
        }
        public override void Dispose()
        {
            BattleEvents.OnAttackResult -= HandleAttack;
        }

        private void HandleAttack(AttackResultEvent e)
        {
            if(e.Attacker != Owner || e.Hit == HitType.Miss)
            {
                return;
            }

            Console.WriteLine($"{Owner.Name}'s passive activates.");
            foreach (Character ally in BattleContext.getAlliesOf(Owner))
            {
                if(ally != Owner && ally.isAlive)
                {
                    ally.CRMeter += 0.15f;
                    Console.WriteLine($"{Owner.Name}'s S2 increases {ally.Name}'s CR by 15%.");
                }
            }
        }
    }
}
