using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTurret : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject turret;
    void OnBecameInvisible()
    {
        Destroy(turret);
    }
}
