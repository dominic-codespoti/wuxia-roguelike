using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI.Main_Menu
{
  class MainMenu : MonoBehaviour
  {
    private VisualElement _root;
    private Button _startButton;
    private Button _quitButton;

    public void Start()
    {
      _root = GetComponent<UIDocument>().rootVisualElement;

      _startButton = _root.Query<Button>("StartButton").First();
      _startButton.clicked += StartGame;

      _quitButton = _root.Query<Button>("QuitButton").First();
      _quitButton.clicked += QuitGame;
    }

    public void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
        QuitGame();
      }
    }

    private void StartGame()
    {
      SceneManager.LoadScene("Level1");
    }

    private void QuitGame()
    {
      Application.Quit();
    }
  }
}
