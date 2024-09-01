using Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game
{
  class Hud : MonoBehaviour
  {
    [field: SerializeField] public Player.Player Player { get; private set; }

    private VisualElement _root;
    private ProgressBar _healthBar;
    private ProgressBar _expBar;

    public void Start()
    {
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

      EventBus.Subscribe<Events.MenuOpened>((evt) => Hide());
      EventBus.Subscribe<Events.MenuClosed>((evt) => Show());
    }

    public void Update()
    {
      Label healthLabel = _healthBar.Q<Label>();
      healthLabel.text = $"{Player.CurrentHealth} / {Player.MaxHealth}";
      _healthBar.value = Player.CurrentHealth;
      _healthBar.lowValue = 0;
      _healthBar.highValue = Player.MaxHealth;

      Label expLabel = _expBar.Q<Label>();
      expLabel.text = $"{Player.Experience} / {PlayerLevelStages.StageExperienceThresholds[Player.CultivationStage]}";
      _expBar.value = Player.Experience;
      _expBar.lowValue = 0;
      _expBar.highValue = PlayerLevelStages.StageExperienceThresholds[Player.CultivationStage];
    }

    public void Hide()
    {
      _root.style.display = DisplayStyle.None;
    }

    public void Show()
    {
      _root.style.display = DisplayStyle.Flex;
    }
  }
}
