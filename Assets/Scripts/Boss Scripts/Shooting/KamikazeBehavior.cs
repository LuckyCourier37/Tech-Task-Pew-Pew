using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeBehavior : MonoBehaviour
{
    public float speed = 5f;               // �������� ��������
    public float explosionRange = 2f;     // ������ ������
    private float explosionDamage = 10f;   // ���� �� ������
    private Transform target;             // ���� (�����)
    private bool isExploded = false;
    [SerializeField]private GameObject Explosion;

    public void SetTarget(Transform target) // ��������� ����
    {
        this.target = target;
    }

    void FixedUpdate()
    {
        if (target == null || isExploded) return;

        // �������� � ����
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // �������� �� ���������� �� ������
        if (Vector3.Distance(transform.position, target.position) <= explosionRange)
        {
            Explode();
        }
    }

    void Explode() // ������� ���������� ������ ���� ����� ������ ����������� � �������
    {
        if (isExploded) return;
        isExploded = true;

        // ���� �� �������
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRange);
        foreach (var obj in hitObjects)
        {
            if (obj.CompareTag("Player"))
            {
                // ������� ���� ������
               obj.GetComponent<PlayerHealth>()?.TakeDamage(explosionDamage);
            }
        }

        Instantiate(Explosion, transform.position, Quaternion.identity);
        // ���������� ������ ������ (������ �������� ��� ������ ������)
        Destroy(gameObject); // ���������� ���������
    }

    void OnDrawGizmosSelected()
    {
        // ������ ������ ������ ��� �������� �������
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

    public void ForcedExplode() 
    {

        Instantiate(Explosion, transform.position, Quaternion.identity);// ���������� ������ ������ (������ �������� ��� ������ ������)

        Destroy(gameObject); // ���������� ���������
    }

   
}
