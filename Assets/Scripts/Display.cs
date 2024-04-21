using UnityEngine;

/// <summary>
/// The HUD.
/// </summary>
class Display : MonoBehaviour 
{
  public PlayerController playerController;

  public void OnGUI()
  {
    // Display the player's movement state.
    GUI.Label(new Rect(Screen.width - 50, 0, 100, 20), playerController.movementState.ToString());
  }
}
