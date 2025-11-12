using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8
{
    public class Stats
    {
        public float  Attack {  get; set; }
        public float Defense { get; set; }
        public float Hp { get; set; }
        public float Speed { get; set; }
        public float CritChance { get; set; }
        public float CritDamage { get; set; }
        public float Effectiveness { get; set; }
        public float EffectResistance { get; set; }
        public float DualAttackChance  { get; set; }

        public Stats(float attack, float defense, float hp, float speed, float critChance, float critDamage, float effectiveness, float effectResistance, float dualAttackChance)
        {
            Attack = attack;
            Defense = defense;
            Hp = hp;
            Speed = speed;
            CritChance = critChance;
            CritDamage = critDamage;
            Effectiveness = effectiveness;
            EffectResistance = effectResistance;
            DualAttackChance = dualAttackChance;
        }

        public Stats Clone () => (Stats)MemberwiseClone ();
    }
}
