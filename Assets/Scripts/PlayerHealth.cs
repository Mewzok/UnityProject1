using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deathScreen;
    private bool isDead = false;

    public TextMeshProUGUI goPointsText;

    void OnTriggerEnter(Collider other)
    {
        if (isDead)
        {
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // disable player control
        var movement = GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.enabled = false;
        }

        // disable collider
        var collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // stop rigidbody just in case
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // explosion effect
        var deathFX = GetComponent<DeathEffect>();
        if (deathFX != null)
        {
            deathFX.TriggerDeathEffect(transform.position);
        }

        // screen shake
        CameraController cam = Camera.main.GetComponent<CameraController>();
        if (cam != null)
        {
            StartCoroutine(cam.Shake(0.3f, 0.15f));
        }

        // hide model so explosion is visible
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        // delay pause and death screen
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        // disable enemies scripts entirely to actually get them to stop please
        EnemyController[] enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        foreach(EnemyController e in enemies) {
            e.OnPlayerDeath();
        }

        // show game over screen
        yield return new WaitForSecondsRealtime(.5f);
        Time.timeScale = 0f;
        if (deathScreen != null)
        {
            // hide in game points ui
            if(PointManager.Instance.pointsText != null) {
                PointManager.Instance.pointsText.gameObject.SetActive(false);
            }

            deathScreen.SetActive(true);

            if(goPointsText != null) {
                Debug.Log("goPointsText is not null");
                goPointsText.text = $"Total Score: {PointManager.Instance.totalPoints}";
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
