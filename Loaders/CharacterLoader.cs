using epic8.Calcs;
using epic8.NPCBehavior;
using epic8.Skills;
using epic8.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace epic8.Loaders
{
    //to be used later when we want to load characters from json.
    public static class CharacterLoader
    {
        public static Character LoadFromFile(string filePath)
        {
            //Convert JSON to CharacterData
            string json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<CharacterData>(json);

            //Parse string to Element enum
            Element element;
            Enum.TryParse(data.Element, out element);

            //Stats object for Character
            Stats stats = new Stats(
                data.BaseStats.Attack,
                data.BaseStats.Defense,
                data.BaseStats.Hp,
                data.BaseStats.Speed,
                data.BaseStats.CritChance,
                data.BaseStats.CritDamage,
                data.BaseStats.Effectiveness,
                data.BaseStats.EffectResistance,
                data.BaseStats.DualAttackChance);

            //NPC Controller for character
            INPCController npc = NPCFactory.Create(data.NPCController);

            //Create the character first, and add skills later
            Character character = new Character(
                data.Name,
                element,
                data.Role,
                stats,
                [],
                ControlType.NPC,
                npc);

            //List of Skills JSON data
            foreach (SkillData skillData in data.Skills)
            {
                //List of SkillEffect data for each skill
                List<SkillEffectData> effectData = skillData.Effects;

                //List that will contain the actual SkillEffects
                List<ISkillEffect> skillEffects = [];

                //Add actual SkillEffects to a list
                foreach (var effect in effectData)
                {
                    ISkillEffect skillEffect = EffectFactory.Create(effect);
                    skillEffects.Add(skillEffect);
                }

                TargetType targetType;
                Enum.TryParse(skillData.TargetType, out targetType);
                //Add the actual Skill to the character
                character.Skills.Add(
                    new Skill(skillData.Name,
                    skillData.Description,
                    skillData.Cooldown,
                    targetType,
                    skillEffects));
            }

            return character;

        }
    }
}
