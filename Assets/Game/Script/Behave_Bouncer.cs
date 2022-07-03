using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behave_Bouncer : MonoBehaviour
{

    private Collider TriggerBox;

    [Range(0, 1000)]
    public float Bounciness;
    [Range(0, 1000)]
    public float Jump;
    [SerializeField]
    private AudioClip Bounce;

    [SerializeField]
    private AudioSource Speaker;



    //private void OnTriggerEnter(Collider collision)
    //{
    //    //  I DONT KNOW WHY THIS WORKS
    //    collision.gameObject.GetComponent<Rigidbody>().velocity =  Vector3.Cross(collision.gameObject.GetComponent<Rigidbody>().velocity,transform.up);


    //    Debug.Log("BOUNCE");
    //    //  I ALSO DONT KNOW WHY THIS DOESNT WORK
    //    //collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.gameObject.GetComponent<Rigidbody>().velocity.normalized * Bounciness);
    //}

    private void OnCollisionEnter(Collision collision)
    {
        //collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.Cross(collision.gameObject.GetComponent<Rigidbody>().velocity, transform.up);

        
        collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.Reflect(collision.gameObject.GetComponent<Rigidbody>().velocity, transform.forward); 

        collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.gameObject.GetComponent<Rigidbody>().velocity * Bounciness + new Vector3(0,1,0) * Jump + transform.forward*Bounciness);

        Speaker.PlayOneShot(Bounce);
    }

}
