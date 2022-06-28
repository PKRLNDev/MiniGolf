using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behave_Hole : MonoBehaviour, IMiniGolf
{
    [SerializeField]
    private Collider CollisionBox;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IMiniGolf Ball))
        {
            Ball.BallSunk();
        }

        
    }

}
