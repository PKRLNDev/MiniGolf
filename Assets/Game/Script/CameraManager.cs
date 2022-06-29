using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
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

    private void BallCamera()
    {
        StateMahchine.m_AnimatedTarget.SetTrigger("LevelReady");
    }
    private void FreeLook()
    {
        StateMahchine.m_AnimatedTarget.SetTrigger("FreeCam");
    }
}
