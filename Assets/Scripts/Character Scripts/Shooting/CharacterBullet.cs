using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // �������� ����
    private Vector2 direction;          // ����������� ��������
    private Rigidbody2D rb;  // ������ �� Rigidbody2D
   
     

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();  // �������� ��������� Rigidbody2D\

        // ������������� ����������� �������
        direction = -transform.right; //  ������ ������� "������" (��������� ��� �) .
                                      // ����� ���������� �������������� ���������� ����������� ������� transform.up � ����������� �������  direction


    }
    void FixedUpdate()
    {

        Vector2 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;
        // ���������� ���� ����� �� � ���������� �����������
        // transform.Translate(Vector3.left * speed * Time.deltaTime);
        rb.MovePosition(newPosition);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            Debug.Log("������ ����� � �����!");
           
            try
            {
                var script2 = other.gameObject.GetComponent<BossMovement>();
                script2.SubstractionHealth();
            }
           catch (NullReferenceException e)
            {
                Debug.Log("������ BossMovement �� ����������: " + e.Message);
            }
            Destroy(gameObject); // ���������� ������

            return;
        }

        if (other.CompareTag("Bomb"))
        {
            Destroy(gameObject);
            other.gameObject.GetComponent<KamikazeBehavior>().ForcedExplode();
        }
    }
}
