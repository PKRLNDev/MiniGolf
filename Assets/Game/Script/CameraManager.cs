using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour, IMiniGolf
{

    [SerializeField]
    Cinemachine.CinemachineStateDrivenCamera StateMahchine;
    [SerializeField]
    Camera MainCamera;
    [SerializeField]
    Cinemachine.CinemachineFreeLook FreeLookCamera;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            BallCamera();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            FreeLook();
        }
    }

    public void BallCamera()
    {
        StateMahchine.m_AnimatedTarget.SetTrigger("LevelReady");
    }
    public void FreeLook()
    {
        StateMahchine.m_AnimatedTarget.SetTrigger("FreeCam");
    }
}
