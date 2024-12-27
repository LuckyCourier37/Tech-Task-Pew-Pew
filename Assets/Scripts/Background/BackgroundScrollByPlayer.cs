using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrollByPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player; // Ссылка на игрока
    public float scrollSpeed = 1f; // Скорость прокрутки

    private Vector3 lastPlayerPosition;

    void Start()
    {
        // Сохраняем начальную позицию игрока
        lastPlayerPosition = player.position;
    }

    void Update()
    {
        Vector3 deltaMovement = player.position - lastPlayerPosition;

        transform.position -= new Vector3(deltaMovement.x, deltaMovement.y, 0) * scrollSpeed;

        foreach (Transform tile in transform)
        {
            if (tile.position.y < Camera.main.transform.position.y - 1f) // Если тайл ниже камеры
            {
                tile.position += new Vector3(0, 20f, 0); // Переместить наверх
            }
        }

        lastPlayerPosition = player.position;
    }
}
