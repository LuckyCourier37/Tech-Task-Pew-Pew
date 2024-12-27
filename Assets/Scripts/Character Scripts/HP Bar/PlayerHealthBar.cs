using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField]private Transform healthFillImage; // ������ �� Image ��� ������� ��
    [SerializeField]private SpriteRenderer GreenFillBar;
    [SerializeField] private PlayerHealth script;
    private float maxHealth = 200f;
    [SerializeField]private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        GreenFillBar.color = Color.green;
        UpdateHealthBar();
    }

   
    public void TakeDamage()
    {
        // ��������� ��������
        currentHealth = script.GetHealth();
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        // ������������ ������� ���������� �������
        // healthFillImage.fillAmount = (float)currentHealth / maxHealth;
        float scale = (float)currentHealth / maxHealth;
        healthFillImage.localScale = new Vector3(scale * 1.25f, 2.25f, 1);
    }
}
