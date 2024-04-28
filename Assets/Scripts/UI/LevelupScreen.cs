using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

class LevelupScreen : PopupScreen
{
    public List<Skill> AvailableSkills;
    public List<PassiveSkill> AvailablePassiveSkills;

    public void Start()
    {
        PlayerEvents.OnPlayerLevelUp += player => OnOpen(player);
    }

    private void OnOpen(Player player)
    {
        OpenPopup();

        var stage = player.CultivationStage;
        var realm = player.CultivationRealm;
        var title = _root.Query<Label>("title").First();
        title.text = $"{realm.ToFriendlyString()}, {stage.ToFriendlyString()} Stage";

        GenerateSkillCards(player);
    }

    private void GenerateSkillCards(Player player)
    {
        var skillCardsContainer = _popupContent.Q<VisualElement>("skill-cards-container");
        skillCardsContainer.Clear();

        var addedSkills = new List<string>();

        for (var i = 0; i < 3; i++)
        {
            var randomSkill = AvailableSkills
                .Where(skill => 
                    !player.GetSkill(skill.Name).HasValue && 
                    !addedSkills.Contains(skill.Name))
                .FirstOrDefault();

            bool isPassiveSkill = Random.value > 0.5f;

            if (isPassiveSkill || randomSkill == null)
            {
                var randomPassiveSkill = AvailablePassiveSkills[Random.Range(0, AvailablePassiveSkills.Count)];
                var skillCard = CreateSkillCard(player, randomPassiveSkill);
                skillCardsContainer.Add(skillCard);
            }
            else
            {
                addedSkills.Add(randomSkill.Name);
                var skillCard = CreateSkillCard(player, randomSkill);
                skillCardsContainer.Add(skillCard);
            }
        }
    }

    private void OnSkillSelected(Player player, PassiveSkill selectedSkill)
    {
        player.LearnPassiveSkill(selectedSkill);
        ClosePopup();
    }

    private void OnSkillSelected(Player player, Skill selectedSkill)
    {
        player.LearnSkill(selectedSkill);
        ClosePopup();
    }

    private VisualElement CreateSkillCard(Player player, Skill skillData)
    {
        var skillCard = new VisualElement();
        skillCard.AddToClassList("skill-card");

        var skillName = new Label(skillData.Name);
        skillName.AddToClassList("skill-name");

        var skillDesc = new Label(skillData.Description);
        skillDesc.AddToClassList("skill-description");

        var skillImage = new VisualElement();
        skillImage.style.backgroundImage = skillData.Image.texture;
        skillImage.AddToClassList("skill-image");

        var skillRarity = new Label(skillData.Rarity.ToFriendlyString());
        skillRarity.AddToClassList("skill-rarity");

        skillCard.Add(skillName);
        skillCard.Add(skillImage);
        skillCard.Add(skillDesc);
        skillCard.Add(skillRarity);

        skillCard.RegisterCallback<ClickEvent>(_ => OnSkillSelected(player, skillData));

        return skillCard;
    }

    private VisualElement CreateSkillCard(Player player, PassiveSkill skillData)
    {
        var skillCard = new VisualElement();
        skillCard.AddToClassList("skill-card");
        
        var existingPassiveSkill = player.GetPassiveSkill(skillData.Name);
        var maxRank = skillData.Values.Count - 1;
        var newRank = existingPassiveSkill.Map(skill => skill.Rank + 1).GetValueOrDefault(0);

        var skillName = new Label($"{skillData.Name} ({newRank}/{maxRank})");
        skillName.AddToClassList("skill-name");

        var skillDesc = new Label(skillData.Description.Replace("{value}", skillData.Values.ElementAtOrDefault(newRank).ToString()));
        skillDesc.AddToClassList("skill-description");

        var skillImage = new VisualElement();
        skillImage.style.backgroundImage = skillData.Image.texture;
        skillImage.AddToClassList("skill-image");

        var skillRarity = new Label(skillData.Rarity.ToFriendlyString());
        skillRarity.AddToClassList("skill-rarity");

        skillCard.Add(skillName);
        skillCard.Add(skillDesc);
        skillCard.Add(skillImage);
        skillCard.Add(skillRarity);

        skillCard.RegisterCallback<ClickEvent>(_ => OnSkillSelected(player, skillData));

        return skillCard;
    }
}
