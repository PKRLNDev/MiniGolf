using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behave_Hole : MonoBehaviour, IMiniGolf
{
    [SerializeField]
    private Collider CollisionBox;

    [SerializeField]
    private AudioClip Sunk;
    
    [SerializeField]
    private AudioSource Speaker;


    [SerializeField]
    ParticleSystem Circle;
    [SerializeField]
    ParticleSystem Hit;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IMiniGolf Ball))
        {            
            Circle.Play();
            Hit.Play();
            Ball.BallSunk();
            Speaker.PlayOneShot(Sunk);
        }

        
    }

}
