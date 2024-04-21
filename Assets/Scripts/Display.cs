using UnityEngine;

/// <summary>
/// The HUD.
/// </summary>
class Display : MonoBehaviour 
{
  public PlayerController playerController;

  public void OnGUI()
  {
    GUI.Label(new Rect(Screen.width - 50, 0, 100, 20), playerController.movementState.ToString());
    GUI.Label(new Rect(Screen.width - 50, 20, 100, 20), playerController.direction.ToString());
  }
}
