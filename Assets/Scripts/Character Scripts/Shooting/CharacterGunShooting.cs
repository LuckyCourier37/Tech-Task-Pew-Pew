using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGunShooting : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bulletPrefab; // ������ �������
    public Transform firePoint;     // ����� ������ �������
    public float fireRate = 1f;     // �������� �������� (��������� � �������)
    private float nextFireTime = 0.3f; // ����� ����� ����������

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextFireTime)
        {
           
                Shoot();
            


            nextFireTime = Time.time + 1f / fireRate; // ������������� ����� ���������� ��������
        }
    }

    void Shoot()
    {
        // ������ ������ � ������� firePoint
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
