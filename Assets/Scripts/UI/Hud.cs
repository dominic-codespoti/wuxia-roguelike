using UnityEngine;
using UnityEngine.UIElements;

class Hud : MonoBehaviour
{
  [field: SerializeField] public Player Player { get; private set; }

  private VisualElement _root;
  private ProgressBar _healthBar;
  private ProgressBar _expBar;

  public void Start()
  {
    _root = GetComponent<UIDocument>().rootVisualElement;

    _healthBar = _root.Query<ProgressBar>("health-bar").First();
    _healthBar.style.backgroundColor = new Color(1f, 0.8f, 0.8f); 

    _expBar = _root.Query<ProgressBar>("exp-bar").First();
    _expBar.style.backgroundColor = new Color(0.8f, 0.8f, 1f);
  }

  public void Update()
  {
    _healthBar.value = Player.CurrentHealth / (float)Player.MaxHealth;

    Label healthLabel = _healthBar.Q<Label>();
    healthLabel.text = $"{Player.CurrentHealth} / {Player.MaxHealth}";

    Label expLabel = _expBar.Q<Label>();
    expLabel.text = $"{Player.Experience} / {PlayerLevelStages.StageExperienceThresholds[Player.CultivationStage]}";
  }
}
