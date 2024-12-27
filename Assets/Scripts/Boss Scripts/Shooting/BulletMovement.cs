using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Скорость пули
    private Vector2 direction;          // Направление движения
    private Rigidbody2D rb;  // Ссылка на Rigidbody2D
    private float BulletDamage = 5f;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();  // Получаем компонент Rigidbody2D\

        // Устанавливаем направление снаряда
        direction = - transform.right; //  снаряд смотрит "Вправо" (локальная ось Х) .
                                  // Здесь происходит автоматическое приведение трехмерного вектора transform.up к двухмерному вектору  direction


    }
    void FixedUpdate()
    {

        Vector2 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;
        // Перемещаем пулю вперёд по её локальному направлению
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
            Destroy(gameObject); // Уничтожаем снаряд
        }
    }
}
