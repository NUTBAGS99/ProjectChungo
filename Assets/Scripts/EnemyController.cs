using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 50;
    public int health;
    public GameObject healthBarPrefab; // assign this in Inspector

    private GameObject healthBarUI; // the instantiated health bar
    private Slider healthSlider; // slider from the prefab

    void Start()
    {
        health = maxHealth;

        if (healthBarPrefab != null)
        {
            // Instantiate the health bar above the enemy
            healthBarUI = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
            healthSlider = healthBarUI.GetComponentInChildren<Slider>();

            // Parent it to the enemy so it follows them
            healthBarUI.transform.SetParent(transform);

            if (healthSlider != null)
            {
                healthSlider.maxValue = 1f;
                healthSlider.minValue = 0f;
                healthSlider.value = 1f; // full health at start
            }
        }
        else
        {
            Debug.LogError("HealthBarPrefab is not assigned on " + gameObject.name);
        }
    }

    void Update()
    {
        if (healthBarUI != null)
        {
            // Position it above the enemy's head
            healthBarUI.transform.position = transform.position + Vector3.up * 2f;

            // Make it always face the camera
            healthBarUI.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        Debug.Log("Enemy took damage: " + damage + " current health: " + health);

        if (healthSlider != null)
        {
            healthSlider.value = (float)health / maxHealth;
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died.");

        if (healthBarUI != null)
        {
            Destroy(healthBarUI);
        }

        Destroy(gameObject);
    }
}