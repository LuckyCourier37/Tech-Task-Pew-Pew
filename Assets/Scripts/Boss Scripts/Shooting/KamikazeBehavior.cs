using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeBehavior : MonoBehaviour
{
    public float speed = 5f;               // Скорость движения
    public float explosionRange = 2f;     // Радиус взрыва
    private float explosionDamage = 10f;   // Урон от взрыва
    private Transform target;             // Цель (игрок)
    private bool isExploded = false;
    [SerializeField]private GameObject Explosion;

    public void SetTarget(Transform target) // Установка цели
    {
        this.target = target;
    }

    void FixedUpdate()
    {
        if (target == null || isExploded) return;

        // Движение к цели
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Проверка на расстояние до игрока
        if (Vector3.Distance(transform.position, target.position) <= explosionRange)
        {
            Explode();
        }
    }

    void Explode() // Функция вызывается только если бомба близко подобралась к объекту
    {
        if (isExploded) return;
        isExploded = true;

        // Урон по области
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRange);
        foreach (var obj in hitObjects)
        {
            if (obj.CompareTag("Player"))
            {
                // Наносим урон игроку
               obj.GetComponent<PlayerHealth>()?.TakeDamage(explosionDamage);
            }
        }

        Instantiate(Explosion, transform.position, Quaternion.identity);
        // Визуальный эффект взрыва (добавь анимацию или префаб взрыва)
        Destroy(gameObject); // Уничтожаем камикадзе
    }

    void OnDrawGizmosSelected()
    {
        // Рисуем радиус взрыва для удобства отладки
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

    public void ForcedExplode() 
    {

        Instantiate(Explosion, transform.position, Quaternion.identity);// Визуальный эффект взрыва (добавь анимацию или префаб взрыва)

        Destroy(gameObject); // Уничтожаем камикадзе
    }

   
}
