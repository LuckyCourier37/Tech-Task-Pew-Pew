using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Префаб снаряда
    public Transform firePoint;     // Точка выхода снаряда
    public float fireRate = 1f;     // Скорость стрельбы (выстрелов в секунду)
    private float nextFireTime = 0.3f; // Время между выстрелами
    private SpriteRenderer turret;
    public int gunIndex { get; set; }            // Индекс пушки
    private bool DoubleLocked;
    private bool isOddTurn { get; set; } = true;   // Определяет, какой сейчас порядок: нечётный (true) или чётный (false)
    private bool PermissinToShoot { get; set; } // Разрешение на стрельбу

    [SerializeField] private GameObject kamikazePrefab; // Префаб врага-камикадзе
   
    [SerializeField] private float kamikazeSpawnChance = 0.04f; // Шанс спавна (30%)
    private BossMovement health { get; set; } // Получение доступа к скрипту главного родителя
    


    private void Start()
    {
        turret = GetComponentInChildren<SpriteRenderer>(); // Получаем рендер текстуры пушки
        
        DoubleLocked = false;

        PermissinToShoot = true;

        health = GetComponentInParent<BossMovement>();
    }
    void FixedUpdate()
    {
        if (health.GetHealth() <= 10f)
        {
            FinalShooting();
            return;
        }
        FirstShooting();
        

    }

    private void FirstShooting() // Начальная стрельба, работает до тех пор пока у босса много ХП
    {
        if (Time.time >= nextFireTime && PermissinToShoot)
        {
            if (ShouldShoot()) // Определяем, стрелять ли пушке в текущем порядке
            {
                Shoot();
            }


            nextFireTime = Time.time + 1f / fireRate; // Устанавливаем время следующего выстрела
        }
    }

    private bool ShouldShoot() // Проверка на разрешение к стрельбе
    {

        OddparityCheck();

        // Нечётные пушки стреляют, если isOddTurn = true
        // Чётные пушки стреляют, если isOddTurn = false
        return (gunIndex % 2 == 1 && isOddTurn) || (gunIndex % 2 == 0 && !isOddTurn);

       
    }

    void Shoot()
    {
       
        if (Random.value < kamikazeSpawnChance && health.GetHealth()  <= health.GetHalfHealthLevel() )
        {                           // Создаем снаряд-камикадзе если уровень здоровья меньше половины
            SpawnKamikaze();
            return;
        }

        // Создаём снаряд в позиции firePoint
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    private void FinalShooting() // Финальная стадия стрельбы
    {
        if (Time.time >= nextFireTime && PermissinToShoot) // Стрельба без проверок но ограничена по времени
        {
              Shoot();
            kamikazeSpawnChance = 0.04f; // Уменьшаем шанс спавна камикадзе вдвое
            nextFireTime = Time.time + 1f / fireRate; // Устанавливаем время следующего выстрела
        }
           
    }

    public void ToggleTurn()
    {
        // Меняем текущий порядок стрельбы
        isOddTurn = !isOddTurn;
    }

    private void OddparityCheck() // Проверка на нечетность
    {
        if (gunIndex % 2 == 1) // Проверка на срабатывание переключателя isOddTurn
        { turret.color = Color.black; } // Меняем цвет текстуры на черный
    }

    public void ProscribeShooting(bool variable) // Функция для запрета стрельбы после отпадания пушки от босса
    {
        PermissinToShoot = variable;
    }

    void SpawnKamikaze() // Функция отвечающая за вызов камикадзе
    {
        if (health.GetPlayerTransform() != null)
        {
            GameObject kamikaze = Instantiate(kamikazePrefab, firePoint.position, Quaternion.identity);
            kamikaze.GetComponent<KamikazeBehavior>().SetTarget(health.GetPlayerTransform());
        }
        else return;
    }

    public void DoubleKamikazeChance() // Удвоение шанса появления камикадзе
    {
        if(!DoubleLocked)
        {
            kamikazeSpawnChance = kamikazeSpawnChance * 2f;
            DoubleLocked = true;
        }
       
    }

   


}
