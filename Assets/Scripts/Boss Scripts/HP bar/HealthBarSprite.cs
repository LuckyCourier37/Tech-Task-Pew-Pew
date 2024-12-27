using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarSprite : MonoBehaviour
{
     [SerializeField]private Transform healthBar; // Ссылка на полоску ХП
    private float maxHealth = 200;
    [SerializeField] BossMovement script;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        if (healthBar != null) 
        {
            TakeDamage();
        }
    }
    public void TakeDamage()
    {
        currentHealth = script.GetHealth();
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Изменяем масштаб полоски ХП
        float scale = (float)currentHealth / maxHealth;
        healthBar.localScale = new Vector3(scale * 1.25f, 2.25f, 1);
    }
}
