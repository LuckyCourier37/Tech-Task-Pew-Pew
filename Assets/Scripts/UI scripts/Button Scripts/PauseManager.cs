using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField]private GameObject ExitButton;// Ссылка на объект меню паузы
    [SerializeField] private GameObject title;
    private bool isPaused = false;

    void Update()
    {
        // Обработка нажатия кнопки паузы
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Останавливаем игровое время
        ExitButton.SetActive(true); // Отображаем кнопку выхода
        title.SetActive(true); // Отображаем заголовок
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Возвращаем игровое время
        ExitButton.SetActive(false); // Скрываем кнопку выхода
        title.SetActive(false); // Скрываем заголовок
        isPaused = false;
    }

    public void QuitGame()
    {
        // Завершение игры
        Application.Quit();
        Debug.Log("Игра завершена!");
    }
}
