using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private CharacterController controller;

    [Header("Projectile")]
    public GameObject fishPrefab;
    public Transform projectileSpawnPoint;
    public float fishSpeed = 10f;
    public float maxDistance = 8f;

    [Header("Attack Cooldown")]
    public float shootCooldown = 0.5f;
    private float lastShootTime = 0f;

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
        // check for death
        if (Time.timeScale == 0f)
        {
            return;
        }

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
        if (Input.GetMouseButtonDown(0) && Time.time >= lastShootTime + shootCooldown)
        {
            ShootFish();
            lastShootTime = Time.time;
        }
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

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Default")))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = transform.position + transform.forward * maxDistance;
        }

        GameObject fish = Instantiate(fishPrefab, projectileSpawnPoint.position, Quaternion.identity);
        fish.GetComponent<FishProjectile>().Initialize(targetPoint);
    }
}
