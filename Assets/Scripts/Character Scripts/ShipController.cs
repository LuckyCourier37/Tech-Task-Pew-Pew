using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Скорость движения

    [Header("Rotation Settings")]
    public float rotationSpeed = 200f; // Скорость вращения
    private Rigidbody2D rb;
    private float spriteWidth = 1.28f; // Размер спрайта по оси Х
    private float spriteHeight = 2.56f; // Размер спрайта по оси У
    private Camera mainCamera;

    private void Start()
    {
        initialization();
    }
    void Update()
    {
      
        HandleRotation();
    }

    private void FixedUpdate()
    {
        MoveShip();
    }
    
    private void HandleRotation()
    {
        // Получаем позицию мыши в мировых координатах
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Вычисляем направление к мыши
        Vector3 direction = mousePosition - transform.position; // Через вычитание векторов получаем вектор направления

        // Устанавливаем Z-координату в 0, чтобы избежать ошибок в 2D
        direction.z = 0f;

        // Вычисляем угол к мыши. Mathf.Atan2(direction.y, direction.x) - вычисляет угол  θ в радианах между вектором direction и осью X.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Угол в градусах

        // Поворачиваем корабль
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f)); // Минус 90° для корректной ориентации. Перевод угла в квантернион.
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // Функция плавно поворачивает корабль к новому квантерниону.
    }

    private void MoveShip()
    {

        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // Получаем ввод от клавиатуры
        Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime; //Используется вместо Time.deltaTime, так как физика Unity обновляется в методе FixedUpdate

        newPosition = ClampToCameraBounds(newPosition);   // Ограничиваем движение в пределах камеры

        rb.MovePosition(newPosition); //Перемещает объект с учётом физики. Если путь к новой позиции перекрыт коллайдером, объект остановится, и столкновение будет обработано.

    }

    Vector2 ClampToCameraBounds(Vector2 position)
    {
        // Получаем границы камеры в мировых координатах
        Vector3 minBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));  // Левая нижняя точка камеры
        Vector3 maxBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));  // Правая верхняя точка камеры

        float offsetX = spriteWidth / 2.5f; // Смещение по оси Х
        float offsetY = spriteHeight / 4.5f; // Смещение по оси У

        // Ограничиваем позицию объекта
        position.x = Mathf.Clamp(position.x, minBounds.x + offsetX, maxBounds.x - offsetX);
        position.y = Mathf.Clamp(position.y, minBounds.y + offsetY, maxBounds.y - offsetY);

        return position;
    }

    private void initialization()
    {
        rb = GetComponent<Rigidbody2D>(); // Получаем компонент Rigidbody2D
        mainCamera = Camera.main; // Получаем главную камеру

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2 size = spriteRenderer.bounds.size;

        Debug.Log("Размер модели: " + size);
    } // Получение компонентов камеры и Rigidbody2D
}
