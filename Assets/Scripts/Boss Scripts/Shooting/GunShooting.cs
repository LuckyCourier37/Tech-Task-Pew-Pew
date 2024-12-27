using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // ������ �������
    public Transform firePoint;     // ����� ������ �������
    public float fireRate = 1f;     // �������� �������� (��������� � �������)
    private float nextFireTime = 0.3f; // ����� ����� ����������
    private SpriteRenderer turret;
    public int gunIndex { get; set; }            // ������ �����
    private bool DoubleLocked;
    private bool isOddTurn { get; set; } = true;   // ����������, ����� ������ �������: �������� (true) ��� ������ (false)
    private bool PermissinToShoot { get; set; } // ���������� �� ��������

    [SerializeField] private GameObject kamikazePrefab; // ������ �����-���������
   
    [SerializeField] private float kamikazeSpawnChance = 0.04f; // ���� ������ (30%)
    private BossMovement health { get; set; } // ��������� ������� � ������� �������� ��������
    


    private void Start()
    {
        turret = GetComponentInChildren<SpriteRenderer>(); // �������� ������ �������� �����
        
        DoubleLocked = false;

        PermissinToShoot = true;

        health = GetComponentInParent<BossMovement>();
    }
    void FixedUpdate()
    {
        if (health.GetHealth() <= 10f)
        {
            FinalShooting();
            return;
        }
        FirstShooting();
        

    }

    private void FirstShooting() // ��������� ��������, �������� �� ��� ��� ���� � ����� ����� ��
    {
        if (Time.time >= nextFireTime && PermissinToShoot)
        {
            if (ShouldShoot()) // ����������, �������� �� ����� � ������� �������
            {
                Shoot();
            }


            nextFireTime = Time.time + 1f / fireRate; // ������������� ����� ���������� ��������
        }
    }

    private bool ShouldShoot() // �������� �� ���������� � ��������
    {

        OddparityCheck();

        // �������� ����� ��������, ���� isOddTurn = true
        // ׸���� ����� ��������, ���� isOddTurn = false
        return (gunIndex % 2 == 1 && isOddTurn) || (gunIndex % 2 == 0 && !isOddTurn);

       
    }

    void Shoot()
    {
       
        if (Random.value < kamikazeSpawnChance && health.GetHealth()  <= health.GetHalfHealthLevel() )
        {                           // ������� ������-��������� ���� ������� �������� ������ ��������
            SpawnKamikaze();
            return;
        }

        // ������ ������ � ������� firePoint
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    private void FinalShooting() // ��������� ������ ��������
    {
        if (Time.time >= nextFireTime && PermissinToShoot) // �������� ��� �������� �� ���������� �� �������
        {
              Shoot();
            kamikazeSpawnChance = 0.04f; // ��������� ���� ������ ��������� �����
            nextFireTime = Time.time + 1f / fireRate; // ������������� ����� ���������� ��������
        }
           
    }

    public void ToggleTurn()
    {
        // ������ ������� ������� ��������
        isOddTurn = !isOddTurn;
    }

    private void OddparityCheck() // �������� �� ����������
    {
        if (gunIndex % 2 == 1) // �������� �� ������������ ������������� isOddTurn
        { turret.color = Color.black; } // ������ ���� �������� �� ������
    }

    public void ProscribeShooting(bool variable) // ������� ��� ������� �������� ����� ��������� ����� �� �����
    {
        PermissinToShoot = variable;
    }

    void SpawnKamikaze() // ������� ���������� �� ����� ���������
    {
        if (health.GetPlayerTransform() != null)
        {
            GameObject kamikaze = Instantiate(kamikazePrefab, firePoint.position, Quaternion.identity);
            kamikaze.GetComponent<KamikazeBehavior>().SetTarget(health.GetPlayerTransform());
        }
        else return;
    }

    public void DoubleKamikazeChance() // �������� ����� ��������� ���������
    {
        if(!DoubleLocked)
        {
            kamikazeSpawnChance = kamikazeSpawnChance * 2f;
            DoubleLocked = true;
        }
       
    }

   


}
