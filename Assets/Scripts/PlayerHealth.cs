using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deathScreen;
    private bool isDead = false;

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
            Debug.Log("Triggering death effect");
            deathFX.TriggerDeathEffect();
        }

        // screen shake
        CameraController cam = Camera.main.GetComponent<CameraController>();
        if (cam != null)
        {
            Debug.Log("Triggering shake effect");
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
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0f;
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
