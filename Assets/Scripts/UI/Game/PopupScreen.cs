using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game
{
  public abstract class PopupScreen : MonoBehaviour
  {
    protected VisualElement _root;
    protected VisualElement _popupContainer;
    protected VisualElement _popupContent;

    public void Awake()
    {
      _root = GetComponent<UIDocument>().rootVisualElement;
      _popupContainer = _root.Query<VisualElement>("popup-container").First();
      _popupContent = _popupContainer.Query<VisualElement>("popup-content").First();

      _popupContainer.style.display = DisplayStyle.None;
    }

    public void OpenPopup()
    {
      _popupContainer.style.display = DisplayStyle.Flex;

      Time.timeScale = 0;
    }

    public void ClosePopup()
    {
      _popupContainer.style.display = DisplayStyle.None;

      Time.timeScale = 1;
    }
  }
}
