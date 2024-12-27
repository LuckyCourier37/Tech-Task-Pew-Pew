using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrollByPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player; // ������ �� ������
    public float scrollSpeed = 1f; // �������� ���������

    private Vector3 lastPlayerPosition;

    void Start()
    {
        // ��������� ��������� ������� ������
        lastPlayerPosition = player.position;
    }

    void Update()
    {
        Vector3 deltaMovement = player.position - lastPlayerPosition;

        transform.position -= new Vector3(deltaMovement.x, deltaMovement.y, 0) * scrollSpeed;

        foreach (Transform tile in transform)
        {
            if (tile.position.y < Camera.main.transform.position.y - 1f) // ���� ���� ���� ������
            {
                tile.position += new Vector3(0, 20f, 0); // ����������� ������
            }
        }

        lastPlayerPosition = player.position;
    }
}
