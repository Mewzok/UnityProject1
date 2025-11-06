using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public GameObject explosionPrefab;

    public void TriggerDeathEffect()
    {
        if (explosionPrefab != null)
        {
            GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }
    }
}
