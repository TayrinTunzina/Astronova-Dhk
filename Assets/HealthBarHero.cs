using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHero : MonoBehaviour
{
    public Slider slider;
    private int maxHealth = 5;
    private int currentHealth;

    void Start()
    {
        SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void DecreaseHealth(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        SetHealth(currentHealth);
    }

    public void DecreaseHealthWithDelay(float delay, int amount)
    {
        StartCoroutine(DecreaseHealthAfterDelay(delay, amount));
    }

    private IEnumerator DecreaseHealthAfterDelay(float delay, int amount)
    {
        yield return new WaitForSeconds(delay);
        DecreaseHealth(amount);
    }
}
