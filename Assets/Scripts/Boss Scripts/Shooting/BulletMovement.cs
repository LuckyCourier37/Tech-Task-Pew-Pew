using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // �������� ����
    private Vector2 direction;          // ����������� ��������
    private Rigidbody2D rb;  // ������ �� Rigidbody2D
    private float BulletDamage = 5f;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();  // �������� ��������� Rigidbody2D\

        // ������������� ����������� �������
        direction = - transform.right; //  ������ ������� "������" (��������� ��� �) .
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(BulletDamage);
            Destroy(gameObject); // ���������� ������
        }
    }
}
