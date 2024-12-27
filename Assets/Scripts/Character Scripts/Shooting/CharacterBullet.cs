using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Скорость пули
    private Vector2 direction;          // Направление движения
    private Rigidbody2D rb;  // Ссылка на Rigidbody2D
   
     

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();  // Получаем компонент Rigidbody2D\

        // Устанавливаем направление снаряда
        direction = -transform.right; //  снаряд смотрит "Вправо" (локальная ось Х) .
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            Debug.Log("Снаряд попал в Босса!");
           
            try
            {
                var script2 = other.gameObject.GetComponent<BossMovement>();
                script2.SubstractionHealth();
            }
           catch (NullReferenceException e)
            {
                Debug.Log("Объект BossMovement не существует: " + e.Message);
            }
            Destroy(gameObject); // Уничтожаем снаряд

            return;
        }

        if (other.CompareTag("Bomb"))
        {
            Destroy(gameObject);
            other.gameObject.GetComponent<KamikazeBehavior>().ForcedExplode();
        }
    }
}
