using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private CharacterController controller;

    [Header("Attack")]
    public GameObject swipePrefab;
    public Transform swipeSpawnPoint;
    public float swipeDuration = 0.2f;

    [Header("Projectile")]
    public GameObject fishPrefab;
    public Transform projectileSpawnPoint;
    public float fishSpeed = 10f;

    public Animator anim;
    private Vector3 moveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        moveDirection = new Vector3(h, 0f, v).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        if (anim != null)
        {
            bool isMoving = moveDirection.magnitude > 0.1f;
            anim.SetBool("isWalking", isMoving);
        }
    }

    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //SwipeAttack();
        }

        if (Input.GetMouseButtonDown(0))
        {
            ShootFish();
        }
    }

    void SwipeAttack()
    {
        Vector3 spawnPos = swipeSpawnPoint != null ? swipeSpawnPoint.position : transform.position + transform.forward * 1f;
        Quaternion spawnRot = transform.rotation;
        GameObject swipe = Instantiate(swipePrefab, spawnPos, spawnRot);
        Destroy(swipe, swipeDuration);
    }

    void ShootFish()
    {
        if (fishPrefab == null)
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 targetPoint;

        if(Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Default")))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = transform.position + transform.forward * 10f;
        }

        Vector3 spawnPos = projectileSpawnPoint != null ? projectileSpawnPoint.position : transform.position + transform.forward * 1f;
        GameObject fish = Instantiate(fishPrefab, spawnPos, transform.rotation);
        Rigidbody rb = fish.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (targetPoint - spawnPos).normalized;
            rb.linearVelocity = dir * fishSpeed;

            fish.transform.forward = dir;
        }
        Destroy(fish, 3f);
    }
}
