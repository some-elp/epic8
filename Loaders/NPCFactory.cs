using epic8.NPCBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Loaders
{
    public static class NPCFactory
    {
        public static INPCController Create(NPCControllerData data)
        {
            switch (data.Type)
            {
                case "BasicNPC":
                    return new BasicNPC();
                case "HealerNPC":
                    return new HealerNPC(data.Parameters["S2Threshold"], data.Parameters["S3Threshold"]);
                default:
                    throw new Exception("Unknown NPC Controller Type");
            }
        }
    }
}
