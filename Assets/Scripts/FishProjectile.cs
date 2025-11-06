using UnityEngine;

public class FishProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float maxDistance = 8f;

    private Vector3 startPos;
    private Vector3 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize(Vector3 targetPos)
    {
        startPos = transform.position;
        direction = (targetPos - startPos).normalized;
        direction.y = 0f;

        // rotate
        transform.rotation = Quaternion.LookRotation(direction);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(startPos, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if(enemy != null) {
                enemy.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
            }
            
            Destroy(gameObject);
        }
    }
}
