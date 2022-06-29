using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUi : MonoBehaviour, IMiniGolf
{
    [SerializeField]
    Cinemachine.CinemachineStateDrivenCamera CameraController;



    public void PlayUIAnim(string AnimName) { CameraController.GetComponent<IMiniGolf>().PlayUIAnim(AnimName); }


}
