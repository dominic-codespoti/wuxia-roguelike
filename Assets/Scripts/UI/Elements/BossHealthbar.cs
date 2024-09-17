using System;
using Common;
using Common.Eventing;
using Entities.Enemy;
using UnityEngine;
using UnityEngine.UIElements;
using World;

namespace UI.Elements
{
    public class BossHealthbar : MonoBehaviour
    {
        private ProgressBar _healthBar;
        private Maybe<Enemy> _currentBoss;
        private Label _bossName;
        private Label _bossTitle;
        private VisualElement _root;

        public void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _healthBar = _root.Query<ProgressBar>().First();
            _bossName = _root.Query<Label>("title").First();
            _bossTitle = _root.Query<Label>("sub-text").First();

            _root.style.display = DisplayStyle.None;
        }

        public void Start()
        {
            _currentBoss = Maybe<Enemy>.None;
            
            var healthProgressBackground = _healthBar.Q<VisualElement>(className: "unity-progress-bar__background");
            healthProgressBackground.style.backgroundColor = Color.gray;
            var healthProgressFill = _healthBar.Q<VisualElement>(className: "unity-progress-bar__progress");
            healthProgressFill.style.backgroundColor = Color.red;
            
            EventBus.Subscribe<Events.BossSpawned>(evt => Show(evt.Enemy, evt.Title, evt.Subtitle));
            EventBus.Subscribe<Events.BossDied>(evt => Hide(evt.Enemy));
        }
        
        public void Update()
        {
            if (_currentBoss.HasValue)
            {
                var boss = _currentBoss.Value;
                _healthBar.value = boss.CurrentHealth;
                _healthBar.lowValue = 0;
                _healthBar.highValue = boss.Stats.Health;
            }
        }
        
        private void Show(Enemy enemy, string title, string subtitle)
        {
            _currentBoss = enemy;
            _bossName.text = title;
            _bossTitle.text = subtitle;
            _root.style.display = DisplayStyle.Flex;
        }

        private void Hide(Enemy enemy)
        {
            if (_currentBoss.HasValue && _currentBoss.Value == enemy)
            {
                _root.style.display = DisplayStyle.None;
            }
        }
    }
}