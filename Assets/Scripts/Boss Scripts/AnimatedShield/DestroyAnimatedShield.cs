using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimatedShield : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Execution");
    }

    // Update is called once per frame
    IEnumerator Execution()
    {
        yield return new WaitForSeconds( 10.25f);
        Destroy(gameObject);
    }
}
