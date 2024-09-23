using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount = 100f;
    [SerializeField] private float drainTime = 0.25f;
    private float target = 1f;
    private Coroutine drainHealthBar;
    [SerializeField] private Gradient healthBarGradient;
    private Color newHealthBarColor;

    public ChargeMeterManager chargeMeterManager;

    void Start()
    {
        CheckHealthBarGradientAmount();
    }

    void Update()
    {
        if (healthAmount <= 0)
        {
            SceneManager.LoadScene("Testing Enviroment");
        }
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        target = healthAmount / 100f;
        drainHealthBar = StartCoroutine(DrainHealthBar());
    }

    private IEnumerator DrainHealthBar()
    {
        float fillAmount = healthBar.fillAmount;
        float elapsedTime = 0f;
        Color currentColor = healthBar.color;

        while (elapsedTime < drainTime)
        {
            elapsedTime += Time.deltaTime;
            healthBar.fillAmount = Mathf.Lerp(fillAmount, target, elapsedTime / drainTime);
            healthBar.color = Color.Lerp(currentColor, newHealthBarColor, elapsedTime / drainTime);
            yield return null;
        }
    }

    private void CheckHealthBarGradientAmount()
    {
        newHealthBarColor = healthBarGradient.Evaluate(target);
    }
}
