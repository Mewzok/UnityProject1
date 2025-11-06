using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    public void TriggerDeathEffect(Vector3 position)
    {
        if (explosionPrefab == null)
        {
            return;
        }

        GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(fx, 2f);
    }
}
