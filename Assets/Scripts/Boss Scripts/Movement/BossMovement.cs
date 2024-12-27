using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;         // �������� ��������
    [SerializeField] private float directionChangeTime = 2f; // ����� ����� ������ �����������
    [SerializeField] private GameObject ShieldRound; // ������ ���������� ������ ����
    [SerializeField] private GameObject AnimatedShieldRound; // ������ ���������� �������������� ������ ����
    [SerializeField] private Sprite example1Partly; // �������� ������������ ������
    [SerializeField] private Sprite example2Fully; // ��������� ����������� ������
    private SpriteRenderer ShieldTexture; // ���������� ��� ������������ ������ �������
    [SerializeField] private BossSetup script; // ������ ������� Shield Round ��� ������ ������� ��������� �����
    [SerializeField] private BossBlink Shieldblink; // ������ ��� ������� Shield Round
    [SerializeField] private BossBlink TowerBlink; // ������ ��� �����
    private Vector2 movementDirection;  // ������� ����������� ��������
    private float nextChangeTime; // ����� ��������� ����������� ��������
    private Rigidbody2D rb;  // ������ �� Rigidbody2D
    private Camera mainCamera;
    private float spriteWidth = 2.92f; // ������ ���������� �� ��� �
    private float spriteHeight = 2.92f; // ������ ���������� �� ��� �
    private float health = 200; // ���������� ����� ��������
    private float HalfHealthLevel { get; set; } // ������� ������� ��������

    private bool toggleToSubstract = true; // ������� ��� ��������� ��������
    private GameObject player; // ������ ������
    [SerializeField] private GameObject TowerExplosion; // ������ �� �������� �������� ���������� ������
    private CircleCollider2D circleCollider;


    void Start()
    {
        initialization();
    }

    void FixedUpdate()
    {
        GeneralMovement();
    }

    void ChangeDirection()
    {
        // ���������� ��������� �����������
        float randomAngle = Random.Range(0f, 360f);
        movementDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
    }

    Vector2 ClampToCameraBounds(Vector2 position) // ���������� �������� ������� ����� ������ ������
    {
        // �������� ������� ������ � ������� �����������
        Vector3 minBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));  // ����� ������ ����� ������
        Vector3 maxBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));  // ������ ������� ����� ������

        float offsetX = spriteWidth / 2.5f;
        float offsetY = spriteHeight / 2.5f;

        // ������������ ������� �������
        position.x = Mathf.Clamp(position.x, minBounds.x + offsetX, maxBounds.x - offsetX);
        position.y = Mathf.Clamp(position.y, minBounds.y + offsetY, maxBounds.y - offsetY);

        return position;
    }

    private void initialization() // ��������� ����������� ������ � Rigidbody2D
    {
        rb = GetComponent<Rigidbody2D>();  // �������� ��������� Rigidbody2D

        // ����� ��������� ��������� �����������
        ChangeDirection();
        nextChangeTime = Time.time + directionChangeTime;

        mainCamera = Camera.main; // �������� ������� ������

        player = GameObject.FindWithTag("Player"); // ����� ������� ������
        
        ShieldTexture = ShieldRound.GetComponent<SpriteRenderer>(); // ��������� ������� ����

        HalfHealthLevel = health / 2; // �������� �� �������� ������ ��������

        // �������� ��������� CircleCollider2D
         


         circleCollider = GetComponent<CircleCollider2D>(); // �������� ��������� CircleCollider2D
       

        Vector2 size = circleCollider.bounds.size;

        Debug.Log("������ ����������: " + size);
    }

    private void GeneralMovement() // ����� ��������
    {
        // ������� ����� � �������������� MovePosition
        Vector2 newPosition = rb.position + movementDirection * moveSpeed * Time.fixedDeltaTime; // �� ��������� �� ������� ��������� ������� ����� rb.position,
                                                                                                 // � �� �������� ����� transform.position.

        newPosition = ClampToCameraBounds(newPosition);   // ������������ �������� � �������� ������

        rb.MovePosition(newPosition);

        // ����� ����������� �� �������
        if (Time.time >= nextChangeTime)
        {
            ChangeDirection();
            nextChangeTime = Time.time + directionChangeTime;
        }
    }

    public void SubstractionHealth() // ��������� �������� �����
    {
        if (toggleToSubstract)
        {
            health -= 5;
            toggleToSubstract = false;
            CheckUpToChangeSprite(health); // �������� �� ��������� �������
            Debug.Log("������ ����� � �����!" + health);

            if (health > 40f) Shieldblink.TakeDamage();
            else if (health <= -20f)
            {
                FinalExplode();
            }

           
             StartCoroutine("Delay");
        }

    }

    IEnumerator Delay() // �������� ��� ��������� ��������
    {
        if (health == 10f) // ��������� �������� �� 10 ������, ��� ������ ��������. ���������� ����������
        {
            AdditionAnamatedShield();
            yield return new WaitForSeconds(10.25f);
            toggleToSubstract = true;
           
        }
        else // ����������� ��������.
        {
            yield return new WaitForSeconds(0.25f);
            toggleToSubstract = true;
        }
       
    }

    private  void AdditionAnamatedShield()
    {
        Instantiate(AnimatedShieldRound, transform.position, Quaternion.identity, transform);
    }


    private void CheckUpToChangeSprite(float value) // �������� �������� ����� ���������� ������� �����
    {
        switch (value)
        {
            case 150f:
                ShieldTexture.sprite = example1Partly; // ��������� ������� ���� �� ����� ���������
                script.FallofCannon(0);
                 break; // ����� ������� ��� ��������� �����
                
            case 100f:
                ShieldTexture.sprite = example2Fully;
                script.FallofCannon(1); break;
            case 50f:
                ShieldTexture.enabled = false;
                script.FallofCannon(2);
                StartCoroutine("DoubleSpawnChance");
                break;
            case 0:
                script.FallofCannon(3);
                circleCollider.radius = 0.8f;
                break;
            default: break;
           
        }
        if (value <= 40f )
        {
            TowerBlink.TakeDamage();
            ShieldTexture.enabled = false;
        }
       
        
    }
    
    public float GetHealth()
    {
        return health;
    }

    public Transform GetPlayerTransform()
    {
        if(player != null) // �������� �� ��������� �� ������
        {
            return player.transform;
        }
        else { return null; }
    }
    

    public float GetHalfHealthLevel()
    {
        return HalfHealthLevel;
    }

    private void FinalExplode()
    {
        Instantiate(TowerExplosion, transform.position, Quaternion.identity);// ���������� ������ ������ (������ �������� ��� ������ ������)
        Destroy(gameObject); // ���������� ������� ������ - �����
    }

    IEnumerator DoubleSpawnChance()
    {
        var Chance = GetComponentsInChildren<GunShooting>();
        foreach (var ch in Chance)
        {
            ch.DoubleKamikazeChance();
        }

        yield return null;
    }
}
