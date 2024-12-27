using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField]private GameObject ExitButton;// ������ �� ������ ���� �����
    [SerializeField] private GameObject title;
    private bool isPaused = false;

    void Update()
    {
        // ��������� ������� ������ �����
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
        Time.timeScale = 0f; // ������������� ������� �����
        ExitButton.SetActive(true); // ���������� ������ ������
        title.SetActive(true); // ���������� ���������
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // ���������� ������� �����
        ExitButton.SetActive(false); // �������� ������ ������
        title.SetActive(false); // �������� ���������
        isPaused = false;
    }

    public void QuitGame()
    {
        // ���������� ����
        Application.Quit();
        Debug.Log("���� ���������!");
    }
}
