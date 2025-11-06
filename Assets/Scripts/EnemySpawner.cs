using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs;
    public float spawnRadius = 20f;
    public float spawnInterval = 2f;

    [Header("Arena Bounds")]
    public Vector2 xBounds = new Vector2(-13.5f, 13.5f);
    public Vector2 zBounds = new Vector2(13.5f, -13.5f);

    private Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || player == null)
        {
            return;
        }

        Vector3 spawnPos;
        int safety = 0;

        do
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
            spawnPos = new Vector3(player.position.x + randomCircle.x, player.position.y, player.position.z + randomCircle.y);

            // clamp in arena bounds
            spawnPos.x = Mathf.Clamp(spawnPos.x, xBounds.x, xBounds.y);
            spawnPos.z = Mathf.Clamp(spawnPos.z, zBounds.x, zBounds.y);

            safety++;
            if (safety > 20) break;
        } while (IsInCameraView(spawnPos));

        // random animal
        int index = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[index], spawnPos, Quaternion.identity);
    }

    bool IsInCameraView(Vector3 worldPos)
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(worldPos);
        return viewportPos.x > 0f && viewportPos.x < 1f && viewportPos.y > 0f && viewportPos.y < 1f && viewportPos.z > 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
