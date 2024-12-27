using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 2f; // Скорость скроллинга
    public float resetPosition = 20f; // Позиция, где фон сбрасывается
    public float startPosition = -20f; // Начальная позиция для повторного появления

    void Update()
    {
        // Двигаем фон вниз
        transform.Translate(Vector2.down * scrollSpeed * Time.deltaTime);

        // Сбрасываем позицию фона, если он вышел за экран
        if (transform.position.y <= resetPosition)
        {
            transform.position = new Vector3(transform.position.x, startPosition, transform.position.z);
        }
    }
}
