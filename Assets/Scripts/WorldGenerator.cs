using UnityEngine;

/// <summary>
/// A 2D world generator. Generates rooms and connects them with hallways.
/// </summary>
class WorldGenerator : MonoBehaviour
{
  public Sprite roomSprite;

  public PlayerController playerController;

  public void Start()
  {
    var point = new Vector2(0, 0);
    GenerateRoom(point);
  }

  private void GenerateRoom(Vector2 point)
  {
    var room = new GameObject("Room");
    room.transform.parent = transform;
    room.transform.position = point;
    room.AddComponent<SpriteRenderer>();
    room.GetComponent<SpriteRenderer>().sprite = roomSprite;
    room.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);

    room.transform.localScale = new Vector3(10, 10, 1);
  }
}
