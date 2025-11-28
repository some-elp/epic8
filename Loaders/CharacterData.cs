using epic8.BuffsDebuffs;
using epic8.Calcs;
using epic8.NPCBehavior;
using epic8.Skills;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Loaders
{
    public class CharacterData
    {
        public string Name { get; set; }
        public string Element { get; set; }
        public string Role { get; set; }
        public Stats BaseStats { get; set; }
        public List<SkillData> Skills { get; set; }
        public NPCControllerData NPCController { get; set; }
    }

    public class NPCControllerData
    {
        public string Type { get; set; }
        public Dictionary<string, float> Parameters { get; set; }
    }

    public class StatsData
    {
        public float Attack {  get; set; }
        public float Defense { get; set; }
        public float Hp {  get; set; }
        public float Speed { get; set; }
        public float CritChance { get; set; }
        public float CritDamage { get; set; }
        public float Effectiveness { get; set; }
        public float EffectResistance { get; set; }
        public float DualAttackChance { get; set; }
    }

    public class SkillData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cooldown { get; set; }
        public string TargetType { get; set; }
        public List<SkillEffectData> Effects { get; set; }
    }

    public class SkillEffectData
    {
        public string EffectType { get; set; }

        public string EffectTargetType { get; set; }
        public DamageParams Damage { get; set; }
        public HealParams Heal { get; set; }
        public BarrierParams Barrier { get; set; }
        public ApplyStatChangeParams StatChange { get; set; }
        public CleanseParams CleanseDebuff { get; set; }
        public CRPushParams CRPush { get; set; }
    }

    public class DamageParams
    {
        public float AtkRate { get; set; }
        public float HpScaling { get; set; }
        public float DefScaling { get; set; }
        public float Power { get; set; }
        public float SkillUps {  get; set; }

        public string PercentFormula { get; set; }
        public string FlatFormula { get; set; }
    }

    public class ApplyStatChangeParams
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public bool IsBuff { get; set; }
        public bool IsDebuff { get; set; }
        public List<StatModifierData> StatModifiers { get; set; }
        public float Chance { get; set; }
    }

    public class HealParams
    {
        public string Formula { get; set; }
    }

    public class BarrierParams
    {
        public string Formula { get; set; }
        public int Duration { get; set; }
    }

    public class CleanseParams
    {
        public int Amount { get; set; }
    }

    public class  CRPushParams
    {
        public float Amount {  get; set; }
    }

    public class StatModifierData
    {
        public string Stat {  get; set; }
        public float PercentChange { get; set; }
        public float FlatChange { get; set; }
    }
}
