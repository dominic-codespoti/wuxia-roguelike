using UnityEngine;

/// <summary>
/// A script that makes an object bob up and down.
/// </summary>
public class Bob : MonoBehaviour
{
    [field: SerializeField] public float minBobSpeed { get; private set; } = 0.5f; // Minimum speed of the bobbing motion
    [field: SerializeField] public float maxBobSpeed { get; private set; } = 2.0f; // Maximum speed of the bobbing motion
    [field: SerializeField] public float bobHeight { get; private set; } = 0.1f; // Height of the bobbing motion

    private Vector3 startPosition;
    private float offset;
    private float bobSpeed;

    void Start()
    {
        startPosition = transform.position;
        offset = Random.Range(-bobHeight, bobHeight);
        bobSpeed = Random.Range(minBobSpeed, maxBobSpeed);
    }

    void Update()
    {
        offset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = startPosition + new Vector3(0f, offset, 0f);
    }
}
