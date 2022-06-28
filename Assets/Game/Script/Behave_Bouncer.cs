using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behave_Bouncer : MonoBehaviour
{

    private Collider TriggerBox;

    [Range(0, 1000)]
    public float Bounciness;



    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * Bounciness);
    }
}
