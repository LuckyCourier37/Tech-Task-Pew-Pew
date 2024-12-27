using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;         // Скорость движения
    [SerializeField] private float directionChangeTime = 2f; // Время между сменой направления
    [SerializeField] private GameObject ShieldRound; // Объект содержащий спрайт Щита
    [SerializeField] private GameObject AnimatedShieldRound; // Объект содержащий анимимрованный спрайт Щита
    [SerializeField] private Sprite example1Partly; // Частично поврежденная ячейка
    [SerializeField] private Sprite example2Fully; // Полностью разрушенная ячейка
    private SpriteRenderer ShieldTexture; // Переменная для присваивания нового спрайта
    [SerializeField] private BossSetup script; // Скрипт объекта Shield Round для вызова функции отделения пушек
    [SerializeField] private BossBlink Shieldblink; // Скрипт для объекта Shield Round
    [SerializeField] private BossBlink TowerBlink; // Скрипт для Башни
    private Vector2 movementDirection;  // Текущее направление движения
    private float nextChangeTime; // Время изменения направления движения
    private Rigidbody2D rb;  // Ссылка на Rigidbody2D
    private Camera mainCamera;
    private float spriteWidth = 2.92f; // Размер коллайдера по оси Х
    private float spriteHeight = 2.92f; // Размер коллайдера по оси У
    private float health = 200; // Количество очков здоровья
    private float HalfHealthLevel { get; set; } // Базовый уровень здоровья

    private bool toggleToSubstract = true; // Тумблер для вычитания здоровья
    private GameObject player; // Объект игрока
    [SerializeField] private GameObject TowerExplosion; // Префаб со объектом анимации финального взрыва
    private CircleCollider2D circleCollider;


    void Start()
    {
        initialization();
    }

    void FixedUpdate()
    {
        GeneralMovement();
    }

    void ChangeDirection()
    {
        // Генерируем случайное направление
        float randomAngle = Random.Range(0f, 360f);
        movementDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
    }

    Vector2 ClampToCameraBounds(Vector2 position) // Ограничить движение Объекта вдоль границ камеры
    {
        // Получаем границы камеры в мировых координатах
        Vector3 minBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));  // Левая нижняя точка камеры
        Vector3 maxBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));  // Правая верхняя точка камеры

        float offsetX = spriteWidth / 2.5f;
        float offsetY = spriteHeight / 2.5f;

        // Ограничиваем позицию объекта
        position.x = Mathf.Clamp(position.x, minBounds.x + offsetX, maxBounds.x - offsetX);
        position.y = Mathf.Clamp(position.y, minBounds.y + offsetY, maxBounds.y - offsetY);

        return position;
    }

    private void initialization() // Получение компонентов камеры и Rigidbody2D
    {
        rb = GetComponent<Rigidbody2D>();  // Получаем компонент Rigidbody2D

        // Задаём начальное случайное направление
        ChangeDirection();
        nextChangeTime = Time.time + directionChangeTime;

        mainCamera = Camera.main; // Получаем главную камеру

        player = GameObject.FindWithTag("Player"); // Поиск объекта игрока
        
        ShieldTexture = ShieldRound.GetComponent<SpriteRenderer>(); // Получение спрайта щита

        HalfHealthLevel = health / 2; // Половина от базового уровня здоровья

        // Получаем компонент CircleCollider2D
         


         circleCollider = GetComponent<CircleCollider2D>(); // Получаем компонент CircleCollider2D
       

        Vector2 size = circleCollider.bounds.size;

        Debug.Log("Размер коллайдера: " + size);
    }

    private void GeneralMovement() // Общее движение
    {
        // Двигаем босса с использованием MovePosition
        Vector2 newPosition = rb.position + movementDirection * moveSpeed * Time.fixedDeltaTime; // Мы опираемся на текущее положение объекта через rb.position,
                                                                                                 // а не напрямую через transform.position.

        newPosition = ClampToCameraBounds(newPosition);   // Ограничиваем движение в пределах камеры

        rb.MovePosition(newPosition);

        // Смена направления по таймеру
        if (Time.time >= nextChangeTime)
        {
            ChangeDirection();
            nextChangeTime = Time.time + directionChangeTime;
        }
    }

    public void SubstractionHealth() // Вычитание здоровья босса
    {
        if (toggleToSubstract)
        {
            health -= 5;
            toggleToSubstract = false;
            CheckUpToChangeSprite(health); // Проверка на изменение спрайта
            Debug.Log("Снаряд попал в Босса!" + health);

            if (health > 40f) Shieldblink.TakeDamage();
            else if (health <= -20f)
            {
                FinalExplode();
            }

           
             StartCoroutine("Delay");
        }

    }

    IEnumerator Delay() // Задержка для вычитания здоровья
    {
        if (health == 10f) // Финальная задержка на 10 секунд, при низком здоровье. Бессмертие фактически
        {
            AdditionAnamatedShield();
            yield return new WaitForSeconds(10.25f);
            toggleToSubstract = true;
           
        }
        else // Стандартная задержка.
        {
            yield return new WaitForSeconds(0.25f);
            toggleToSubstract = true;
        }
       
    }

    private  void AdditionAnamatedShield()
    {
        Instantiate(AnimatedShieldRound, transform.position, Quaternion.identity, transform);
    }


    private void CheckUpToChangeSprite(float value) // Проверка здоровья перед изменением спрайта босса
    {
        switch (value)
        {
            case 150f:
                ShieldTexture.sprite = example1Partly; // Изменение спрайта щита на менее целостный
                script.FallofCannon(0);
                 break; // Вызов функции для отделения пушек
                
            case 100f:
                ShieldTexture.sprite = example2Fully;
                script.FallofCannon(1); break;
            case 50f:
                ShieldTexture.enabled = false;
                script.FallofCannon(2);
                StartCoroutine("DoubleSpawnChance");
                break;
            case 0:
                script.FallofCannon(3);
                circleCollider.radius = 0.8f;
                break;
            default: break;
           
        }
        if (value <= 40f )
        {
            TowerBlink.TakeDamage();
            ShieldTexture.enabled = false;
        }
       
        
    }
    
    public float GetHealth()
    {
        return health;
    }

    public Transform GetPlayerTransform()
    {
        if(player != null) // Проверка не уничтожен ли объект
        {
            return player.transform;
        }
        else { return null; }
    }
    

    public float GetHalfHealthLevel()
    {
        return HalfHealthLevel;
    }

    private void FinalExplode()
    {
        Instantiate(TowerExplosion, transform.position, Quaternion.identity);// Визуальный эффект взрыва (добавь анимацию или префаб взрыва)
        Destroy(gameObject); // Уничтожаем главный объект - Босса
    }

    IEnumerator DoubleSpawnChance()
    {
        var Chance = GetComponentsInChildren<GunShooting>();
        foreach (var ch in Chance)
        {
            ch.DoubleKamikazeChance();
        }

        yield return null;
    }
}
