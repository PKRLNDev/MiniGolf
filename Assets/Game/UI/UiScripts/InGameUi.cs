using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUi : MonoBehaviour, IMiniGolf
{
    [SerializeField]
    Cinemachine.CinemachineStateDrivenCamera CameraController;


    public void FreeLook()
    {
        CameraController.GetComponent<IMiniGolf>().FreeLook();
    }

    public void BallCamera() 
    {
        CameraController.GetComponent<IMiniGolf>().BallCamera();
    }


}
