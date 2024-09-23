using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public HealthManager healthManager; // Reference to the HealthManager

    void Start()
    {
        if (healthManager == null)
        {
            healthManager = GetComponent<HealthManager>(); // Get HealthManager attached to the opponent
        }
    }

    // Method to take damage
    public void TakeDamage(float damage)
    {
        if (healthManager != null)
        {
            healthManager.TakeDamage(damage);
            Debug.Log("Opponent took damage: " + damage);
            CheckHealth();
        }
    }

    private void CheckHealth()
    {
        if (healthManager.healthAmount <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Opponent has died.");
        // Implement death logic here (e.g., disable, destroy, play animation)
        Destroy(gameObject); // Example: destroy the opponent object
    }
}
