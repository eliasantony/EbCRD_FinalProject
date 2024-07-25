using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float maxHealth = 100;
    public Image healthBar;

    [Header("Damage Overlay")]
    public Image overlay;
    public float duration;
    public float fadeSpeed;
    private float durationTimer;
    
    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        if(overlay.color.a > 0)
        {
            if (health < 30)
                return;
            durationTimer += Time.deltaTime;
            if(durationTimer > duration)
            {
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, overlay.color.a - fadeSpeed * Time.deltaTime);
            }
        }
    }

    private void UpdateHealthUI()
    {
        healthBar.fillAmount = health / maxHealth;
    }
    
    IEnumerator AnimateHealthChange(float newHealth)
    {
        Debug.Log("Animating health change");
        float elapsed = 0;
        float duration = 0.5f; // duration of the animation in seconds
        float oldHealth = health;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            health = Mathf.Lerp(oldHealth, newHealth, elapsed / duration);
            yield return null;
        }
        health = newHealth;
    }

    public void TakeDamage(float damage)
    {
        float newHealth = Mathf.Max(health - damage, 0);
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
        StartCoroutine(AnimateHealthChange(newHealth));
    }

    public void Heal(float healAmount)
    {
        float newHealth = Mathf.Min(health + healAmount, maxHealth);
        StartCoroutine(AnimateHealthChange(newHealth));
    }
}
