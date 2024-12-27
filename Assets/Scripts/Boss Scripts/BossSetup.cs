using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class BossSetup : MonoBehaviour
{
    public GameObject gunPrefab; // Префаб пушки
    [SerializeField] private int gunCount = 16; // Количество пушек
    [SerializeField] private float radius = 2f; // Радиус круга
    [SerializeField] private float rotationSpeed = 30f; // Скорость вращения вдоль окружности
    [SerializeField] private float turnDuration = 5f; // Продолжительность одной прогрессии

    private GunShooting[] guns; // Массив скриптов пушек
    private GunShooting[] gunsObject; // Массив скриптов пушек
    private float nextTurnTime; // Time.time - возвращает общее время, прошедшее с момента запуска сцены, в секундах.

    void Start()
    {
        CreationCannonAndDisposition();
    }

    void Update()
    {
        Rotation();
    }

    private void FixedUpdate()
    {
        Shooting();
    }

    private void CreationCannonAndDisposition() // Создание пушек и расположение их вдоль окружности
    {
        guns = new GunShooting[gunCount];

        for (int i = 0; i < gunCount; i++)
        {
            // Рассчитываем угол
            float angle = i * Mathf.PI * 2 / gunCount;
            Vector3 position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            // Создаём пушку
            GameObject gun = Instantiate(gunPrefab, transform.position + position, Quaternion.identity, transform);

            // Поворачиваем пушку к центру
            gun.transform.up = position.normalized;

            // Назначаем индекс пушке
            GunShooting gunShooting = gun.GetComponent<GunShooting>();
            gunShooting.gunIndex = i + 1;
            guns[i] = gunShooting;
        }

        nextTurnTime = Time.time + turnDuration;

    }

    private void Rotation() // Ротация пушек и изменение очередности стрельбы
    {
        // Вращаем пушки вокруг центра (по окружности)
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

       
    }

    private void Shooting()
    {
        if (Time.time >= nextTurnTime)
        {
            var filteredMass = guns.Where(x => x != null).ToArray();
            // Переключаем порядок стрельбы
            foreach (GunShooting gun in filteredMass)
            {
                gun.ToggleTurn();
            }
            nextTurnTime = Time.time + turnDuration; // Устанавливаем момент времени, когда должна произойти следующая стрельба.
        }
    }

    public void FallofCannon(int value) // Функция отвечающая за откидывание пушки от босса
    {
        switch (value)
        {

            case 0:
                RemoveGuns(1); // Пушки номер 1 и 15
                RemoveGuns(15); break;
            case 1:
                RemoveGuns(4);
                RemoveGuns(12); break; 
            case 2:
                RemoveGuns(9);
                RemoveGuns(10); break;
            case 3:
                var filteredGuns = guns.Where(x => x != null).ToArray();
                // Переключаем порядок стрельбы
                foreach (GunShooting gun in filteredGuns)
                {
                    RemoveGuns(gun.gunIndex);
                }
                break;
        }
    }

    private void RemoveGuns(int index) // Удаление пушки из родительского объекта "Shield Round HEX.."
    {
        try         // Проверка на генерацию исключения " IndexOutOfRangeException" во избежания маскирующего исключения
        {
            guns[index - 1].gameObject.GetComponent<Rigidbody2D>().simulated = true; // Включение компонента Rigidbody2D
            guns[index - 1].gameObject.transform.parent = null; // Разрыв родственных связей 
            guns[index - 1].ProscribeShooting(false); // Воспрещение стрельбы
            guns[index - 1] = null; // Обнуление конкретного элемента массива
           
        }
        catch (IndexOutOfRangeException e) 
        {
            Debug.Log("Подходящий элемент не найден: " + e.Message);
        }

       
    }
}
