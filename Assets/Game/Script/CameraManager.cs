using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour, IMiniGolf
{

    [SerializeField]
    Cinemachine.CinemachineStateDrivenCamera StateMachine;
    [SerializeField]
    Camera MainCamera;
    [SerializeField]
    Cinemachine.CinemachineFreeLook NearCamera;
    [SerializeField]
    Cinemachine.CinemachineFreeLook FarCamera;


    // Update is called once per frame
    void Update()
    {

        // DEBUGINPUT
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayCamAnim("LevelReady");
            
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayCamAnim("NearCamera");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayCamAnim("FarCamera");
        }
    }



    #region IMiniGolf

    public void PlayCamAnim(string AnimName) { StateMachine.m_AnimatedTarget.SetTrigger(AnimName); }



    #endregion
}
