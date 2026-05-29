using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI Components")]
    public Slider healthSlider;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip damageSound;
    //public AudioClip deathSound;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar();

        if (currentHealth > 0)
        {
            PlaySound(damageSound);
        }
       /* else
        {
            Die();
        }*/
    }

    void UpdateHealthBar()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    /*void Die()
    {
        PlaySound(deathSound);

        // Optional: Disable collider or mesh to let the death sound finish playing
        GetComponent<Collider>().enabled = false;

        // Destroy the enemy object after the sound finishes (e.g., 1.5 seconds)
        Destroy(gameObject, 1.5f);
    }*/
}
