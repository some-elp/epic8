using epic8.Calcs;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.EventClasses
{
    public record AttackResultEvent(Character Attacker, Character Target, HitType Hit);
}
