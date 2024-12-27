using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class BossSetup : MonoBehaviour
{
    public GameObject gunPrefab; // ������ �����
    [SerializeField] private int gunCount = 16; // ���������� �����
    [SerializeField] private float radius = 2f; // ������ �����
    [SerializeField] private float rotationSpeed = 30f; // �������� �������� ����� ����������
    [SerializeField] private float turnDuration = 5f; // ����������������� ����� ����������

    private GunShooting[] guns; // ������ �������� �����
    private GunShooting[] gunsObject; // ������ �������� �����
    private float nextTurnTime; // Time.time - ���������� ����� �����, ��������� � ������� ������� �����, � ��������.

    void Start()
    {
        CreationCannonAndDisposition();
    }

    void Update()
    {
        Rotation();
    }

    private void FixedUpdate()
    {
        Shooting();
    }

    private void CreationCannonAndDisposition() // �������� ����� � ������������ �� ����� ����������
    {
        guns = new GunShooting[gunCount];

        for (int i = 0; i < gunCount; i++)
        {
            // ������������ ����
            float angle = i * Mathf.PI * 2 / gunCount;
            Vector3 position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            // ������ �����
            GameObject gun = Instantiate(gunPrefab, transform.position + position, Quaternion.identity, transform);

            // ������������ ����� � ������
            gun.transform.up = position.normalized;

            // ��������� ������ �����
            GunShooting gunShooting = gun.GetComponent<GunShooting>();
            gunShooting.gunIndex = i + 1;
            guns[i] = gunShooting;
        }

        nextTurnTime = Time.time + turnDuration;

    }

    private void Rotation() // ������� ����� � ��������� ����������� ��������
    {
        // ������� ����� ������ ������ (�� ����������)
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

       
    }

    private void Shooting()
    {
        if (Time.time >= nextTurnTime)
        {
            var filteredMass = guns.Where(x => x != null).ToArray();
            // ����������� ������� ��������
            foreach (GunShooting gun in filteredMass)
            {
                gun.ToggleTurn();
            }
            nextTurnTime = Time.time + turnDuration; // ������������� ������ �������, ����� ������ ��������� ��������� ��������.
        }
    }

    public void FallofCannon(int value) // ������� ���������� �� ����������� ����� �� �����
    {
        switch (value)
        {

            case 0:
                RemoveGuns(1); // ����� ����� 1 � 15
                RemoveGuns(15); break;
            case 1:
                RemoveGuns(4);
                RemoveGuns(12); break; 
            case 2:
                RemoveGuns(9);
                RemoveGuns(10); break;
            case 3:
                var filteredGuns = guns.Where(x => x != null).ToArray();
                // ����������� ������� ��������
                foreach (GunShooting gun in filteredGuns)
                {
                    RemoveGuns(gun.gunIndex);
                }
                break;
        }
    }

    private void RemoveGuns(int index) // �������� ����� �� ������������� ������� "Shield Round HEX.."
    {
        try         // �������� �� ��������� ���������� " IndexOutOfRangeException" �� ��������� ������������ ����������
        {
            guns[index - 1].gameObject.GetComponent<Rigidbody2D>().simulated = true; // ��������� ���������� Rigidbody2D
            guns[index - 1].gameObject.transform.parent = null; // ������ ����������� ������ 
            guns[index - 1].ProscribeShooting(false); // ����������� ��������
            guns[index - 1] = null; // ��������� ����������� �������� �������
           
        }
        catch (IndexOutOfRangeException e) 
        {
            Debug.Log("���������� ������� �� ������: " + e.Message);
        }

       
    }
}
