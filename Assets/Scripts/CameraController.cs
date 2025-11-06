using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -3);
    public float smoothSpeed = 5f;

    private Vector3 shakeOffset = Vector3.zero;
    private Coroutine shakeRoutine;

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // base position
        Vector3 desiredPos = target.position + offset;

        // smooth follow
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPos + shakeOffset;
    }

    public void TriggerShake(float duration, float magnitude)
    {
        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
        }
        shakeRoutine = StartCoroutine(Shake(duration, magnitude));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            shakeOffset = new Vector3(x, y, 0);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        shakeOffset = Vector3.zero;
        shakeRoutine = null;
    }
}
