 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Returner : MonoBehaviour, IMiniGolf
{
    [Range(0, 10)]
    public int StayLimit;

    [SerializeField]
    private bool bShouldStop=true;
    [SerializeField]
    private bool bSticky=false;



    private void OnTriggerEnter(Collider other)
    {
        if (bSticky && other.gameObject.TryGetComponent(out IMiniGolf Ball))
        {
            Ball.BallStuck();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IMiniGolf Ball))
        {
            Ball.BallStay(bShouldStop, StayLimit);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (bSticky && collision.gameObject.TryGetComponent(out IMiniGolf Ball))
        {
            Ball.BallUnStuck();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (bSticky && other.gameObject.TryGetComponent(out IMiniGolf Ball))
        {
            Ball.BallUnStuck();
        }
    }
}
