using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float life = 0.4f;
    void Start()
    {
        Destroy(gameObject, life);
    }
}
