using UnityEngine;

/// <summary>
/// The HUD.
/// </summary>
class Display : MonoBehaviour 
{
  public PlayerController playerController;
  public Player player;

  public void OnGUI()
  {
    GUI.Label(new Rect(Screen.width - 50, 0, 100, 20), playerController.MovementState.ToString());
    GUI.Label(new Rect(Screen.width - 50, 20, 100, 20), playerController.Direction.ToString());
    GUI.Label(new Rect(Screen.width - 50, 40, 100, 20), $"Level {player.Level.ToString()}");
  }
}
