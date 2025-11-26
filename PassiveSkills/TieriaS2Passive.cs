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
        private readonly Character _owner;
        private readonly BattleContext _context;

        public TieriaS2Passive(Character owner, BattleContext context)
        {
            _owner = owner;
            BattleEvents.OnAttackResult += HandleAttack;
            _context = context;
        }

        public override void Dispose()
        {
            BattleEvents.OnAttackResult -= HandleAttack;
        }

        private void HandleAttack(AttackResultEvent e)
        {
            if(e.Attacker != _owner || e.Hit == HitType.Miss)
            {
                return;
            }

            Console.WriteLine($"{_owner.Name}'s passive activates.");
            foreach (Character ally in _context.getAlliesOf(_owner))
            {
                if(ally != _owner && ally.isAlive)
                {
                    ally.CRMeter += 0.15f;
                    Console.WriteLine($"{_owner.Name}'s S2 increases {ally.Name}'s CR by 15%.");
                }
            }
        }
    }
}
