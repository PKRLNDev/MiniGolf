using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUi : MonoBehaviour, IMiniGolf
{
    [SerializeField]
    Cinemachine.CinemachineStateDrivenCamera CameraController;

    private IMiniGolf CameraInterface;



    private void Start()
    {
        GetCameraInterface();
    }

    private bool GetCameraInterface()
    {
        if (CameraController.TryGetComponent(out IMiniGolf _CameraInterface))
        {
            CameraInterface = _CameraInterface;
            return true;
        }
        return false;
    }

    public void PlayCamAnim(string AnimName) { if (CameraInterface != null || GetCameraInterface() ) { CameraInterface.PlayCamAnim(AnimName); }  }
    public void PlayUiAnim(string AnimName) { if (CameraInterface != null || GetCameraInterface()) { CameraInterface.PlayCamAnim(AnimName); }  }


}
