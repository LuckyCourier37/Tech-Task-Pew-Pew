using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private MainCamera mainCamera;
    [SerializeField] private GameObject ShipPlayer;
    [SerializeField] private GameObject ShipBoss;
    [SerializeField] private GameObject BackGroundContainer;
    [SerializeField] private Button Exit;
    [SerializeField] private GameObject title;
    public void OnButtonClick()
    {
        mainCamera.ChangePosition();
        ShipBoss.SetActive(true);
        ShipPlayer.SetActive(true);
        BackGroundContainer.SetActive(true);
        Exit.gameObject.SetActive(false);
        gameObject.SetActive(false);
        title.gameObject.SetActive(false);
    }
}
