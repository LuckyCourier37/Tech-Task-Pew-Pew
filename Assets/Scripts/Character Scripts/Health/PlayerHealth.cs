using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    private float ship_Health = 200f;
    [SerializeField] private GameObject Explosion;
    [SerializeField] private PlayerHealthBar script;
    private void FixedUpdate()
    {
        if ( ship_Health <= 0 )
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);// Визуальный эффект взрыва (добавь анимацию или префаб взрыва)
            Destroy(gameObject );
        }
    }

    // Update is called once per frame


    public void TakeDamage(float Damage)
    {
        ship_Health  -= Damage;
        Debug.Log("Снаряд столкнулся с игроком!" + ship_Health);
        script.TakeDamage();
    }

    public float GetHealth() { return ship_Health;}
}
