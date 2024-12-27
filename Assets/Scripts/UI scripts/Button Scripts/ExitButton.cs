using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnExitButtonClick()
    {
        Debug.Log("Игра завершена!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Завершает воспроизведение в редакторе
#else
        Application.Quit(); // Завершает игру в сборке
#endif
    }
}
