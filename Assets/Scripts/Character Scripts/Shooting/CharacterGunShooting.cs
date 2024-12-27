using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGunShooting : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bulletPrefab; // Префаб снаряда
    public Transform firePoint;     // Точка выхода снаряда
    public float fireRate = 1f;     // Скорость стрельбы (выстрелов в секунду)
    private float nextFireTime = 0.3f; // Время между выстрелами

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextFireTime)
        {
           
                Shoot();
            


            nextFireTime = Time.time + 1f / fireRate; // Устанавливаем время следующего выстрела
        }
    }

    void Shoot()
    {
        // Создаём снаряд в позиции firePoint
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
