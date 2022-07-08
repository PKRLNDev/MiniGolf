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

    public IMiniGolf BallInterface;

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("UnSunkBall").TryGetComponent(out IMiniGolf _interface))
        {
            BallInterface = _interface;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
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

        */
    }



    #region IMiniGolf

    public void PlayCamAnim(string AnimName) { StateMachine.m_AnimatedTarget.SetTrigger(AnimName); }

    public void OnBallReady() { BallInterface.OnBallReady(); }

    #endregion
}
