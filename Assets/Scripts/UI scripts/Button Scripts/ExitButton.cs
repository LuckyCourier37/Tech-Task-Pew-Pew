using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnExitButtonClick()
    {
        Debug.Log("���� ���������!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ��������� ��������������� � ���������
#else
        Application.Quit(); // ��������� ���� � ������
#endif
    }
}
