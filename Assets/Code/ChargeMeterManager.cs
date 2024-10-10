using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChargeMeterManager : MonoBehaviour
{
    public Image playerChargeMeter;                
    public GameObject playerChargedMeterUI;       
    public GameObject playerActivatedMeter;        

    public Image enemyChargeMeter;                
    public GameObject enemyChargedMeterUI;       
    public GameObject enemyActivatedMeter;        

    public float playerChargeAmount = 0f; 
    public float enemyChargeAmount = 0f;  
    [SerializeField] private float maxChargeAmount = 10f;
    [SerializeField] private float chargeTime = 0.25f; // Smooth time to fill charge meter
    [SerializeField] private float drainTime = 0.25f;  

    private Coroutine playerChargingMeter;  // Separate coroutine for player
    private Coroutine enemyChargingMeter;   // Separate coroutine for enemy
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

        healthManager = FindObjectOfType<HealthManager>();
        if (healthManager == null)
        {
            Debug.LogError("HealthManager not found in the scene!");
        }
    }

    public void AdjustPlayerChargeMeter(float chargeCount)
    {
        if (!isPerformingSpecialAttack)  
        {
            float previousAmount = playerChargeAmount;  // Save previous value for smooth transition
            playerChargeAmount = Mathf.Clamp(playerChargeAmount + chargeCount, 0, maxChargeAmount);

            if (playerChargingMeter != null)
            {
                StopCoroutine(playerChargingMeter); // Stop previous player coroutine
            }
            playerChargingMeter = StartCoroutine(SmoothChargeMeter(playerChargeMeter, previousAmount, playerChargeAmount, chargeTime));

            if (playerChargeAmount >= maxChargeAmount)
            {
                ActivatePlayerChargedMeter();
            }
        }
    }

    public void AdjustEnemyChargeMeter(float chargeCount)
    {
        if (!isPerformingSpecialAttack) 
        {
            float previousAmount = enemyChargeAmount;  // Save previous value for smooth transition
            enemyChargeAmount = Mathf.Clamp(enemyChargeAmount + chargeCount, 0, maxChargeAmount);

            if (enemyChargingMeter != null)
            {
                StopCoroutine(enemyChargingMeter);  // Stop previous enemy coroutine
            }
            enemyChargingMeter = StartCoroutine(SmoothChargeMeter(enemyChargeMeter, previousAmount, enemyChargeAmount, chargeTime));

            if (enemyChargeAmount >= maxChargeAmount)
            {
                ActivateEnemyChargedMeter();
            }
        }
    }

    private IEnumerator SmoothChargeMeter(Image chargeMeter, float startAmount, float endAmount, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            chargeMeter.fillAmount = Mathf.Lerp(startAmount / maxChargeAmount, endAmount / maxChargeAmount, elapsedTime / duration);
            yield return null;
        }
        chargeMeter.fillAmount = endAmount / maxChargeAmount;  // Ensure exact value at the end
    }

    private void ActivatePlayerChargedMeter()
    {
        playerChargedMeterUI.SetActive(true);          
        playerChargeMeter.gameObject.SetActive(false);  
    }

    private void ActivateEnemyChargedMeter()
    {
        enemyChargedMeterUI.SetActive(true);           
        enemyChargeMeter.gameObject.SetActive(false);   
    }

    public void ActivateSpecialAttack(bool isPlayer)
    {
        if (!isPerformingSpecialAttack && IsFullyCharged(isPlayer))
        {
            StartCoroutine(SpecialAttackSequence(isPlayer));
        }
    }

    private IEnumerator SpecialAttackSequence(bool isPlayer)
    {
        Debug.Log("Activating special attack sequence...");
        isPerformingSpecialAttack = true;

        if (isPlayer)
        {
            playerChargedMeterUI.SetActive(false);
            playerActivatedMeter.SetActive(true);
        }
        else
        {
            enemyChargedMeterUI.SetActive(false);
            enemyActivatedMeter.SetActive(true);
        }

        yield return new WaitForSeconds(3f);

        if (healthManager != null)
        {
            healthManager.TakeDamage(7, !isPlayer);
            Debug.Log(isPlayer ? "Player Performing Special Attack..." : "Opponent Performing Special Attack...");
        }

        yield return new WaitForSeconds(1f);

        if (isPlayer)
        {
            playerActivatedMeter.SetActive(false);
            ResetPlayerChargeMeter();
        }
        else
        {
            enemyActivatedMeter.SetActive(false); 
            ResetEnemyChargeMeter();            
        }

        isPerformingSpecialAttack = false;  
    }

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

    public void ResetEnemyChargeMeter()
    {
        enemyChargedMeterUI.SetActive(false);          
        enemyChargeMeter.gameObject.SetActive(true);   

        if (drainingMeter != null)
        {
            StopCoroutine(drainingMeter);
        }

        drainingMeter = StartCoroutine(DrainChargeMeter(enemyChargeMeter, (amount) => enemyChargeAmount = amount, enemyChargeAmount));        
    }

    private IEnumerator DrainChargeMeter(Image chargeMeter, Action<float> updateChargeAmount, float chargeAmount)
    {
        float initialChargeAmount = chargeAmount;
        float elapsedTime = 0f;

        while (elapsedTime < drainTime)
        {
            elapsedTime += Time.deltaTime;
            chargeAmount = Mathf.Lerp(initialChargeAmount, 0f, elapsedTime / drainTime);
            updateChargeAmount(chargeAmount); 
            UpdateChargeMeters();  
            yield return null;
        }

        chargeAmount = 0f;
        updateChargeAmount(chargeAmount); 
        UpdateChargeMeters();
    }

    private void UpdateChargeMeters()
    {
        playerChargeMeter.fillAmount = playerChargeAmount / maxChargeAmount;
        enemyChargeMeter.fillAmount = enemyChargeAmount / maxChargeAmount;
    }

    public bool IsFullyCharged(bool isPlayer)
    {
        return isPlayer ? playerChargeAmount >= maxChargeAmount : enemyChargeAmount >= maxChargeAmount; 
    }
}
