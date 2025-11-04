using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Combat")]
    public GameObject sliceEffectPrefab;
    public Transform sliceSpawnPoint;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        AttackInput();
    }

    void Move() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;

        if(dir.magnitude > 0.01f) {
            transform.forward = dir;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }

        //anim.SetFloat("Speed", dir.magnitude);
    }

    void AttackInput() {
        if(Input.GetMouseButtonDown(0)) {
            MeleeAttack();
        }

        if(Input.GetMouseButtonDown(1)) {
            RangedAttack();
        }
    }

    void MeleeAttack() {
        //anim.SetTrigger("Slice");

        if(sliceEffectPrefab != null && sliceSpawnPoint != null) {
            // instantiate the slash prefab
            GameObject slash = Instantiate(sliceEffectPrefab, sliceSpawnPoint.position, sliceSpawnPoint.rotation);

            // start rotation coroutine
            StartCoroutine(SlashArc(slash.transform));
        }
    }

    void RangedAttack() {
        //anim.SetTrigger("Spit");
        if(projectilePrefab != null && projectileSpawnPoint != null) {
            Instantiate(projectilePrefab, projectileSpawnPoint.position, transform.rotation);
        }
    }

    IEnumerator SlashArc(Transform slash) {
        float duration = 0.2f;
        float elapsed = 0f;
        float startAngle = -90f;
        float endAngle = 90f;

        while(elapsed < duration) {
            float t = elapsed / duration;
            float angle = Mathf.Lerp(startAngle, endAngle, t);
            slash.localRotation = Quaternion.Euler(0, angle, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(slash.gameObject);
    }
}
