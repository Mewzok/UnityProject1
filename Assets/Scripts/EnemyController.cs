using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int pointValue = 1;
    private Transform player;
    private Animator animator;
    private bool isDead = false;
    private bool playerDead = false;

    private DeathEffect deathEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        animator = GetComponent<Animator>();
        deathEffect = GetComponent<DeathEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead || playerDead || player == null)
        {
            return;
        }

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        // move enemy
        if(!playerDead) {
            transform.position += direction * moveSpeed * Time.deltaTime;

            // rotate towards player
            if (direction.sqrMagnitude > 0f) {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            // walk animation
            animator.SetBool("isWalking", direction.sqrMagnitude > 0.01f);
        }
    }

    void Die() {
        if(isDead) {
            return;
        }
        isDead =  true;

        if(deathEffect != null) {
            deathEffect.TriggerDeathEffect(transform.position);
        }

        CameraController cam = Camera.main.GetComponent<CameraController>();
        if(cam != null) {
            cam.TriggerShake(0.2f, 0.1f);
        }

        Destroy(gameObject);

        // add points
        if(PointManager.Instance != null) {
            PointManager.Instance.AddPoints(pointValue);
        }
    }

    public void OnPlayerDeath() {
        playerDead = true;
        moveSpeed = 0f;

        if(animator != null) {
            animator.applyRootMotion = false;
            animator.enabled = false;
        }
    }
}
