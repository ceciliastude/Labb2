using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeMeterManager : MonoBehaviour
{
    // Charge meter for player
    public Image playerChargeMeter;                
    public GameObject playerChargedMeterUI;       
    public GameObject playerActivatedMeter;        

    // Charge meter for opponent
    public Image enemyChargeMeter;                
    public GameObject enemyChargedMeterUI;       
    public GameObject enemyActivatedMeter;        

    public float playerChargeAmount = 0f; // Player's charge amount
    public float enemyChargeAmount = 0f;  // Enemy's charge amount
    [SerializeField] private float maxChargeAmount = 10f;
    [SerializeField] private float chargeTime = 0.25f;
    [SerializeField] private float enemyChargeTime = 0.25f; // Add this variable for enemy charge time
    [SerializeField] private float drainTime = 0.25f;  // Drain time for the charge reset
    private float target = 1f;
    private Coroutine chargingMeter;
    private Coroutine enemyChargingMeter; // Coroutine for enemy charging
    private Coroutine drainingMeter;

    private bool isPerformingSpecialAttack = false;
    private HealthManager healthManager;

    void Start()
    {
        playerChargeAmount = 0f;
        enemyChargeAmount = 0f;
        UpdateChargeMeters();
        playerChargedMeterUI.SetActive(false); 
        playerActivatedMeter.SetActive(false);
        enemyChargedMeterUI.SetActive(false); 
        enemyActivatedMeter.SetActive(false);

        // Find the HealthManager component in the scene
        healthManager = FindObjectOfType<HealthManager>();
        if (healthManager == null)
        {
            Debug.LogError("HealthManager not found in the scene!");
        }
    }

    // Method to adjust the player's charge meter
    public void AdjustPlayerChargeMeter(float chargeCount)
    {
        if (!isPerformingSpecialAttack)  // Only charge when not performing special attack
        {
            playerChargeAmount += chargeCount;
            playerChargeAmount = Mathf.Clamp(playerChargeAmount, 0, maxChargeAmount); // Keep charge between 0 and max

            target = playerChargeAmount / maxChargeAmount;
            if (chargingMeter != null)
            {
                StopCoroutine(chargingMeter); // Stop any running charge coroutine
            }
            chargingMeter = StartCoroutine(Charger(playerChargeMeter));

            // Check if charge is full
            if (playerChargeAmount >= maxChargeAmount)
            {
                ActivatePlayerChargedMeter();
            }
        }
    }

    // Method to adjust the enemy's charge meter
    public void AdjustEnemyChargeMeter(float chargeCount)
    {
        if (enemyChargingMeter != null)
        {
            StopCoroutine(enemyChargingMeter); // Stop any running enemy charge coroutine
        }
        enemyChargingMeter = StartCoroutine(EnemyCharger(chargeCount));
    }

    // Coroutine to handle the enemy charging process
    private IEnumerator EnemyCharger(float chargeCount)
    {
        float elapsedTime = 0f;
        float fillAmount = enemyChargeMeter.fillAmount;

        // Gradually charge the enemy meter over the set charge time
        while (elapsedTime < enemyChargeTime)
        {
            elapsedTime += Time.deltaTime;
            enemyChargeAmount += (chargeCount / 2) * (Time.deltaTime / enemyChargeTime); // Charge at half rate
            enemyChargeAmount = Mathf.Clamp(enemyChargeAmount, 0, maxChargeAmount);
            enemyChargeMeter.fillAmount = enemyChargeAmount / maxChargeAmount;
            yield return null;
        }

        // Final update
        enemyChargeAmount = Mathf.Clamp(enemyChargeAmount, 0, maxChargeAmount);
        if (enemyChargeAmount >= maxChargeAmount)
        {
            ActivateEnemyChargedMeter();
        }
    }

    // Activates the "charged" meter UI when fully charged for player
    private void ActivatePlayerChargedMeter()
    {
        playerChargedMeterUI.SetActive(true);            // Show the charged UI
        playerChargeMeter.gameObject.SetActive(false);   // Hide the normal charge meter
    }

    // Activates the "charged" meter UI when fully charged for enemy
    private void ActivateEnemyChargedMeter()
    {
        enemyChargedMeterUI.SetActive(true);            // Show the charged UI
        enemyChargeMeter.gameObject.SetActive(false);   // Hide the normal charge meter
    }

    // Method to start the special attack sequence
    public void ActivateSpecialAttack()
    {
        if (!isPerformingSpecialAttack && IsFullyCharged())
        {
            StartCoroutine(SpecialAttackSequence());
        }
    }

    // Coroutine to handle the special attack sequence
    private IEnumerator SpecialAttackSequence()
    {
        Debug.Log("Activating special attack sequence...");
        isPerformingSpecialAttack = true;

        // Hide the charged meter and show the activated meter
        playerChargedMeterUI.SetActive(false);
        playerActivatedMeter.SetActive(true);

        // Wait for 3 seconds (special attack duration)
        yield return new WaitForSeconds(3f);

        // Perform the special attack (e.g., damage)
        if (healthManager != null)
        {
            healthManager.TakeDamage(7);  // Example: Take 7 damage during special attack
            Debug.Log("Performing Special Attack...");
        }

        // Wait for 1 second after the special attack
        yield return new WaitForSeconds(1f);

        // Hide the activated meter and reset the charge meter
        Debug.Log("Resetting meter...");
        playerActivatedMeter.SetActive(false);
        ResetPlayerChargeMeter();

        isPerformingSpecialAttack = false;  // Re-enable player input
    }

    // Resets the charge meter for the player with a gradual draining effect
    public void ResetPlayerChargeMeter()
    {
        playerChargedMeterUI.SetActive(false);          
        playerChargeMeter.gameObject.SetActive(true);   

        if (drainingMeter != null)
        {
            StopCoroutine(drainingMeter);
        }

        drainingMeter = StartCoroutine(DrainChargeMeter(playerChargeMeter, (amount) => playerChargeAmount = amount, playerChargeAmount));
    }   

    // Gradually drains the charge meter
    private IEnumerator DrainChargeMeter(Image chargeMeter, Action<float> updateChargeAmount, float chargeAmount)
    {
        float initialChargeAmount = chargeAmount;
        float elapsedTime = 0f;

        // Gradually drain the charge meter over the set drain time
        while (elapsedTime < drainTime)
        {
            elapsedTime += Time.deltaTime;
            chargeAmount = Mathf.Lerp(initialChargeAmount, 0f, elapsedTime / drainTime);
            updateChargeAmount(chargeAmount); // Use a delegate to update the charge amount
            UpdateChargeMeters();  // Update the charge meter UI based on the new charge amount
            yield return null;
        }

        // Reset charge meter UI
        chargeAmount = 0f;
        updateChargeAmount(chargeAmount); // Reset the charge amount
        UpdateChargeMeters();
        playerChargedMeterUI.SetActive(false);           // Hide charged meter
        chargeMeter.gameObject.SetActive(true);    // Show normal meter again
    }

    // Coroutine to animate the charging process
    private IEnumerator Charger(Image chargeMeter)
    {
        float fillAmount = chargeMeter.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < chargeTime)
        {
            elapsedTime += Time.deltaTime;
            chargeMeter.fillAmount = Mathf.Lerp(fillAmount, target, elapsedTime / chargeTime);
            yield return null;
        }
    }

    // Updates both charge meters' fill amounts
    private void UpdateChargeMeters()
    {
        playerChargeMeter.fillAmount = playerChargeAmount / maxChargeAmount;
        enemyChargeMeter.fillAmount = enemyChargeAmount / maxChargeAmount;
    }

    // Check if the player's charge meter is fully charged
    public bool IsFullyCharged()
    {
        return playerChargeAmount >= maxChargeAmount;
    }
}
