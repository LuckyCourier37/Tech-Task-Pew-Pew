using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBlink: MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int blinkCount = 3;         // ���������� ��������
    public float blinkInterval = 0.1f; // �������� ����� ����������
    [SerializeField] BossMovement BossMovement;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage()
    {
        StartCoroutine(BlinkEffect());
    }

    private IEnumerator BlinkEffect()
    {
        
            for (int i = 0; i < blinkCount; i++)
            {
                spriteRenderer.enabled = false; // ��������� ������
                yield return new WaitForSeconds(blinkInterval);
                spriteRenderer.enabled = true; // �������� ������
                yield return new WaitForSeconds(blinkInterval);
            }
   
        
    }
}
