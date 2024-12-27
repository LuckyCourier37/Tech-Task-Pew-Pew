using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    public void DestroyObject()
    {
        Destroy(gameObject); // Удаляет объект из сцены
    }
}
