using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleweedScript : MonoBehaviour
{

    public Rigidbody RB;
    public Vector3 AngularVel;
    public Vector3 Vel;

    float LifeSpan = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(gameObject, LifeSpan);
        Vel = transform.rotation* Vel;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RB.angularVelocity = Vel;
        Vel = Vel + (Vel * Time.fixedDeltaTime);
    }
}
