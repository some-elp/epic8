using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.NPCBehavior
{
    public class HealerNPC
    {

        /*
         * 0. Specific: Use a healing skill when relevant
         * 1. ignore invincible targets (not important for now)
         * 2. Attack target where we have elemental advantage
         * 3. Attack lowest %hp target
         * 4. Random target
         * 
         * S3 -> S2 -> S1 skill prio
         * 
         * If there are multiple units that we can hit with elemental advantage
         * Hit the one with lowest %hp.
         * If they are the same hp, select randomly between those targets
         * 
         * Future: 40% chance to hit front target, 20% for rest
         */
    }
}
