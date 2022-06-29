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
            PlayUIAnim("LevelReady");
            
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayUIAnim("FreeCam");
        }
    }



    #region IMiniGolf


    public void PlayUIAnim(string AnimName) { StateMahchine.m_AnimatedTarget.SetTrigger(AnimName); }


    #endregion
}
