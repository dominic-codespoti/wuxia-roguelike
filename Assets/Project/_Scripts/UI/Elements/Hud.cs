using Project._Scripts.Common.Eventing;
using Project._Scripts.Entities.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project._Scripts.UI.Elements
{
  class Hud : MonoBehaviour
  {
    public Minimap minimapManager;

    private Player _player;
    private VisualElement _root;
    private ProgressBar _healthBar;
    private ProgressBar _expBar;

    public void Start()
    {
      _player = FindObjectOfType<Player>();
      _root = GetComponent<UIDocument>().rootVisualElement;

      _healthBar = _root.Query<ProgressBar>("health-bar").First();
      var healthProgressBackground = _healthBar.Q<VisualElement>(className: "unity-progress-bar__background");
      healthProgressBackground.style.backgroundColor = Color.gray;
      var healthProgressFill = _healthBar.Q<VisualElement>(className: "unity-progress-bar__progress");
      healthProgressFill.style.backgroundColor = Color.red;

      _expBar = _root.Query<ProgressBar>("exp-bar").First();
      var expProgressBackground = _expBar.Q<VisualElement>(className: "unity-progress-bar__background");
      expProgressBackground.style.backgroundColor = Color.gray;
      var expProgressFill = _expBar.Q<VisualElement>(className: "unity-progress-bar__progress");
      expProgressFill.style.backgroundColor = Color.yellow;

      EventBus.Subscribe<Events.LevelGenerated>((evt) => ConfigureMinimap());
      EventBus.Subscribe<Events.MenuOpened>((evt) => Hide());
      EventBus.Subscribe<Events.MenuClosed>((evt) => Show());
      EventBus.Subscribe<Events.BossSpawned>((evt) => minimapManager.Hide());
      EventBus.Subscribe<Events.BossDied>((evt) => minimapManager.Show());
    }

    public void Update()
    {
      UpdateHealthBar();
      UpdateExpBar();
    }
    
    public void Hide()
    {
      _root.style.display = DisplayStyle.None;
    }

    public void Show()
    {
      _root.style.display = DisplayStyle.Flex;
    }

    private void ConfigureMinimap()
    {
      minimapManager.Initialize(_root);
    }

    private void UpdateHealthBar()
    {
      Label healthLabel = _healthBar.Q<Label>();
      healthLabel.text = $"{_player.CurrentHealth} / {_player.Stats.Health}";
      _healthBar.value = _player.CurrentHealth;
      _healthBar.lowValue = 0;
      _healthBar.highValue = _player.Stats.Health;
    }

    private void UpdateExpBar()
    {
      Label expLabel = _expBar.Q<Label>();
      expLabel.text = $"{_player.Experience} / {PlayerLevelTable.GetExperienceNeededForLevel(_player.Level)}";
      _expBar.value = _player.Experience;
      _expBar.lowValue = 0;
      _expBar.highValue = PlayerLevelTable.GetExperienceNeededForLevel(_player.Level);
    }
  }
}