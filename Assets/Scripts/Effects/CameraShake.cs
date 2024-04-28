using System.Collections;
using UnityEngine;

/// <summary>
/// A camera shake script that shakes the camera for a given duration and magnitude.
/// </summary>
class CameraShake : MonoBehaviour
{
    private Vector3 _initialLocalPosition;

    public void Start()
    {
        _initialLocalPosition = transform.localPosition;
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        var elapsed = 0f;

        while (elapsed < duration)
        {
            var x = Random.Range(-1f, 1f) * magnitude;
            var y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, _initialLocalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _initialLocalPosition;
    }
}
