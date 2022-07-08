using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleWeedSpawner : MonoBehaviour
{
    public GameObject ThumbleWeed;
    float Timer = 5.0f;

    // Start is called before the first frame update
    void Start()
    {

        

    }



    private void FixedUpdate()
    {
        Timer = Timer - Time.fixedDeltaTime;

        if (Timer > 0) { return; }
        
        Instantiate(ThumbleWeed,transform);
        Timer = 5.0f;
    }

}
