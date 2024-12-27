using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 2f; // �������� ����������
    public float resetPosition = 20f; // �������, ��� ��� ������������
    public float startPosition = -20f; // ��������� ������� ��� ���������� ���������

    void Update()
    {
        // ������� ��� ����
        transform.Translate(Vector2.down * scrollSpeed * Time.deltaTime);

        // ���������� ������� ����, ���� �� ����� �� �����
        if (transform.position.y <= resetPosition)
        {
            transform.position = new Vector3(transform.position.x, startPosition, transform.position.z);
        }
    }
}
