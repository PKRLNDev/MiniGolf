using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUi : MonoBehaviour, IMiniGolf
{
    [SerializeField]
    Cinemachine.CinemachineStateDrivenCamera CameraController;

    private IMiniGolf CameraInterface;
    [SerializeField]
    Animator AnimController;

    [SerializeField]
    private TextMeshProUGUI ScoreText;

    private bool bExitReady = false;

    private void Start()
    {
        GetCameraInterface();
    }

    private bool GetCameraInterface() { if (CameraController.TryGetComponent(out IMiniGolf _CameraInterface))  { CameraInterface = _CameraInterface; return true; }  return false; }




    public void PlayCamAnim(string AnimName) { if (CameraInterface != null || GetCameraInterface() ) { CameraInterface.PlayCamAnim(AnimName); }  }
    public void PlayUiAnim(string AnimName) { if (CameraInterface != null || GetCameraInterface()) { CameraInterface.PlayCamAnim(AnimName); }  }

    public void UpdateScore(int HitCount) { ScoreText.text = HitCount.ToString(); }
    public void UpdateText(string NewText) { ScoreText.text = NewText; }

    public void EndGameOpen() { AnimController.Play("EndGameUiComeAnim", 1); } //AnimController.SetBool("bOpenEndUi", true); AnimController.Play("EndGameUiComeAnim", 1);
    public void EndGameClose() { AnimController.Play("EndGameUiGoAnim",1); }

    public void EndGame() { bExitReady = true; }

    public bool GetExitReady() { return bExitReady; }

    public void OnGameEnded() { EndGameOpen(); }


    #region Buttons

    // TODO Add LevelSelect
    public void BTN_MainMenu() { SceneManager.LoadScene(0); }
    public void BTN_LevelSelect() { SceneManager.LoadScene(0); }
    public void BTN_NextLevel() { SceneManager.LoadScene(1); }

    public void BTN_Pause() 
    {

    }


    #endregion

}
