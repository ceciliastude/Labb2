using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public Image playerHealthBar;
    public Image enemyHealthBar;
    public float playerHealthAmount = 100f;
    public float enemyHealthAmount = 100f;
    [SerializeField] private float drainTime = 0.25f;
    private float targetPlayer = 1f;
    private float targetEnemy = 1f;
    private Coroutine drainHealthBar;
    [SerializeField] private Gradient healthBarGradient;
    private Color newHealthBarColor;

    public ChargeMeterManager chargeMeterManager;

    void Start()
    {
        UpdateHealthBarColor(playerHealthBar, playerHealthAmount);
        UpdateHealthBarColor(enemyHealthBar, enemyHealthAmount);
    }

    void Update()
    {
        if (playerHealthAmount <= 0 && !FindObjectOfType<Player>().isDefeated)
        {
            Debug.Log("Player died.");
            FindObjectOfType<Player>().HandleDeath(true);
            FindObjectOfType<CameraManager>().ZoomOnDefeatedFighter(true);
        }

        if (enemyHealthAmount <= 0 && !FindObjectOfType<Opponent>().isDefeated)
        {
            Debug.Log("Opponent died.");
            FindObjectOfType<Opponent>().HandleDeath(false);
            FindObjectOfType<CameraManager>().ZoomOnDefeatedFighter(false);
        }
    }



    public void TakeDamage(float damage, bool isPlayer)
    {
        if (isPlayer)
        {
            playerHealthAmount -= damage;
            targetPlayer = playerHealthAmount / 100f;
            drainHealthBar = StartCoroutine(DrainHealthBar(playerHealthBar, targetPlayer));
        }
        else
        {
            enemyHealthAmount -= damage;
            targetEnemy = enemyHealthAmount / 100f;
            drainHealthBar = StartCoroutine(DrainHealthBar(enemyHealthBar, targetEnemy));
        }
    }

    private IEnumerator DrainHealthBar(Image healthBar, float target)
    {
        float fillAmount = healthBar.fillAmount;
        float elapsedTime = 0f;
        Color currentColor = healthBar.color;

        newHealthBarColor = healthBarGradient.Evaluate(target);

        while (elapsedTime < drainTime)
        {
            elapsedTime += Time.deltaTime;
            healthBar.fillAmount = Mathf.Lerp(fillAmount, target, elapsedTime / drainTime);
            healthBar.color = Color.Lerp(currentColor, newHealthBarColor, elapsedTime / drainTime);
            yield return null;
        }
    }

    private void UpdateHealthBarColor(Image healthBar, float healthAmount)
    {
        float target = healthAmount / 100f;
        newHealthBarColor = healthBarGradient.Evaluate(target);
        healthBar.color = newHealthBarColor;
    }

    private void DestroyOpponent()
    {
        // Logic for destroying the opponent, like removing the opponent object, playing animations, etc.
        Debug.Log("Opponent destroyed.");
    }
}
