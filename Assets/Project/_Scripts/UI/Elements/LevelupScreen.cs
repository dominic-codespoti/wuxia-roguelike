using System.Collections.Generic;
using System.Linq;
using Project._Scripts.Common;
using Project._Scripts.Common.Eventing;
using Project._Scripts.Entities.Combat;
using Project._Scripts.Entities.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project._Scripts.UI.Elements
{
    public class LevelupScreen : PopupScreen
    {
        private List<PassiveSkill> _availablePassiveSkills;

        public void Start()
        {
            EventBus.Subscribe<Events.PlayerLeveledUp>((evt) => OnOpen(evt.Player));
            
            _availablePassiveSkills = Resources.LoadAll<PassiveSkill>("Passive Skills").ToList();
        }

        private void OnOpen(Player player)
        {
            EventBus.Publish(new Events.MenuOpened());

            OpenPopup();

            var title = _root.Query<Label>("title").First();
            title.text = $"Level {player.Level} - Choose a skill to learn";

            GenerateSkillCards(player);
        }

        private void GenerateSkillCards(Player player)
        {
            var skillCardsContainer = _popupContent.Q<VisualElement>("skill-cards-container");
            skillCardsContainer.Clear();

            for (var i = 0; i < 3; i++)
            {
                var randomSkill = _availablePassiveSkills[Random.Range(0, _availablePassiveSkills.Count)];
                var skillCard = CreateSkillCard(player, randomSkill);
                skillCardsContainer.Add(skillCard);
            }
        }

        private VisualElement CreateSkillCard(Player player, PassiveSkill skillData)
        {
            var skillCard = new VisualElement();
            skillCard.AddToClassList("skill-card");
        
            var existingPassiveSkill = player.GetPassiveSkill(skillData.name);
            var maxRank = skillData.values.Count - 1;
            var newRank = existingPassiveSkill.Map(skill => skill.rank + 1).GetValueOrDefault(0);

            var skillName = new Label($"{skillData.name} ({newRank}/{maxRank})");
            skillName.AddToClassList("skill-name");

            var skillDesc = new Label(skillData.description.Replace("{value}", skillData.values.ElementAtOrDefault(newRank).ToString()));
            skillDesc.AddToClassList("skill-description");

            var skillImage = new VisualElement();
            skillImage.style.backgroundImage = skillData.image != null ? new StyleBackground(skillData.image) : null;
            skillImage.AddToClassList("skill-image");

            var skillRarity = new Label(skillData.rarity.ToFriendlyString());
            skillRarity.AddToClassList("skill-rarity");

            skillCard.Add(skillName);
            skillCard.Add(skillDesc);
            skillCard.Add(skillImage);
            skillCard.Add(skillRarity);

            skillCard.RegisterCallback<ClickEvent>(_ => OnSkillSelected(player, skillData));

            return skillCard;
        }

        private void OnSkillSelected(Player player, PassiveSkill selectedSkill)
        {
            EventBus.Publish(new Events.PassiveSkillLearned(selectedSkill));
            EventBus.Publish(new Events.MenuClosed());
            ClosePopup();
        }
    }
}
