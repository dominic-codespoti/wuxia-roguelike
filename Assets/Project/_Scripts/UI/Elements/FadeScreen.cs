using Project._Scripts.Common.Eventing;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project._Scripts.UI.Elements
{
    public class FadeScreen : PopupScreen
    {
        private Image _fadePanelImage;

        public void Start()
        {
            _fadePanelImage = new Image
            {
                style =
                {
                    backgroundColor = new StyleColor(new Color(0, 0, 0, 0)),
                    position = Position.Absolute,
                    left = 0,
                    top = 0,
                    right = 0,
                    bottom = 0,
                }
            };
            
            _popupContainer.Add(_fadePanelImage);
            
            EventBus.Subscribe<Events.FadeStarted>(evt => OnOpen());
            EventBus.Subscribe<Events.FadeEnded>(evt => OnClose());
            EventBus.Subscribe<Events.FadeAdjusted>((evt) => AdjustFade(evt.Color));
        }
        
        private void OnOpen()
        {
            OpenPopup();
        }
        
        private void OnClose()
        {
            ClosePopup();
        }
        
        private void AdjustFade(Color color)
        {
            _fadePanelImage.style.backgroundColor = new StyleColor(color);
        }
    }
}