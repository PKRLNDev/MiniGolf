using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUi : MonoBehaviour, IMiniGolf
{
    #region Variables

    
    [SerializeField]
    Cinemachine.CinemachineStateDrivenCamera CameraController;

    [SerializeField]
    private Vector2 StartGrabLocation = new Vector2(0, -195);

    private bool bExitReady = false;
    private bool bPaused = false;
    private bool bLevelSelect = false;
    private bool bEndGame = false;

    #endregion

    #region References

    [SerializeField]
    private GameObject BallGrabImage;
    [SerializeField]
    private GameObject Ball;
    [SerializeField]
    private AudioSource Speaker;
    [SerializeField]
    private AudioClip ClickSound;

    private IMiniGolf BallInterface;
    private IMiniGolf CameraInterface;


    #endregion

    #region UiReferences

    [SerializeField]
    private GameObject PauseMenuUi;
    [SerializeField]
    private GameObject LevelSelectUi;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    Animator AnimController;

    [SerializeField]
    private TextMeshProUGUI ScoreText;
    [SerializeField]
    private TextMeshProUGUI WinConditionText;
    [SerializeField]
    private TextMeshProUGUI EndScoreText;
    [SerializeField]
    private TextMeshProUGUI EndTimeText;

    [SerializeField]
    private RectTransform HitBar;
    [SerializeField]
    private RectTransform PullBar;

    [SerializeField]
    private Image HitbarImage;
    [SerializeField]
    private Image PullbarImage;
    #endregion


    private void Start()
    {
        PauseMenuUi.SetActive(false);
        UnPause();
        GetBallInterface();

        GetCameraInterface();

        HitbarImage = HitBar.gameObject.GetComponent<Image>();
        PullbarImage = PullBar.gameObject.GetComponent<Image>();

        PauseMenuUi.SetActive(false);
        //LevelSelectUi.GetComponent<Animator>().Play("LevelSelectAway");


    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (bLevelSelect)
            {
                BTN_LevelSelect();
                return;
            }
            if (bEndGame)
            {
                BTN_MainMenu();
                return;
            }
            BTN_Pause();
        }


    }

    #region Get
    private bool GetBallInterface() { if (Ball.TryGetComponent(out IMiniGolf _BallInterface)) { BallInterface = _BallInterface; return true; } return false; }

    private bool GetCameraInterface() { if (CameraController.TryGetComponent(out IMiniGolf _CameraInterface))  { CameraInterface = _CameraInterface; return true; }  return false; }

    public bool GetExitReady() { return bExitReady; }

    public void EndGame() { bExitReady = true; }

    #endregion

    #region UIManagement

    public void PlayCamAnim(string AnimName) { if (CameraInterface != null || GetCameraInterface() ) { CameraInterface.PlayCamAnim(AnimName); }  }
    public void PlayCamAnim(string AnimName, int Layer) { if (CameraInterface != null || GetCameraInterface() ) { CameraInterface.PlayCamAnim(AnimName,Layer); }  }
    public void PlayUiAnim(string AnimName) { if (AnimController) { AnimController.Play(AnimName); } }
    public void PlayUiAnim(string AnimName, int Layer) { if (AnimController) { AnimController.Play(AnimName, Layer); } }

    public void EndGameOpen() { AnimController.Play("EndGameUiComeAnim", 1); bEndGame = true; } //AnimController.SetBool("bOpenEndUi", true); AnimController.Play("EndGameUiComeAnim", 1);
    public void EndGameClose() { AnimController.Play("EndGameUiGoAnim", 1); }

    public void UpdateScore(int HitCount) { ScoreText.text = HitCount.ToString(); }
    public void UpdateText(string NewText) { ScoreText.text = NewText; }
 
    public void OnGameEnded(int HitCount) 
    { 
        EndScoreText.text = EndScoreText.text + HitCount.ToString(); 
        EndTimeText.text = EndTimeText.text + (Time.timeSinceLevelLoad / 60).ToString("F2"); 
        EndGameOpen(); 
    }

    //public void PlayBallGrabGo() { AnimController.Play("BallGrabGo"); }
    //public void PlayBallGrabCome() { AnimController.Play("BallGrabCome"); }

    #endregion


    #region Buttons

    // TODO Add LevelSelect
    public void BTN_MainMenu() { UnPause(); SceneManager.LoadScene(0); }
    public void BTN_LevelSelect() 
    {
        UnPause(); 

        if (bLevelSelect)
        {
            bLevelSelect = false;
            LevelSelectUi.GetComponent<Animator>().Play("LevelSelectAway");
            if (bEndGame)
            {
                EndGameOpen();
                return;
            }
            PauseMenuUi.SetActive(true);
            Pause();
            return;
        }

        
        LevelSelectUi.GetComponent<Animator>().Play("LevelSelectStay");
        bLevelSelect = true;
        if (bEndGame)
        {
            EndGameClose();
            return;
        }
        PauseMenuUi.SetActive(false);
        Pause();
    }
    public void BTN_NextLevel() 
    {
        UnPause();
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings-1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }
        
        SceneManager.LoadScene(0); 

    }

    public void BTN_Pause() 
    {
        if (bLevelSelect)
        {
            UnPause();
            LevelSelectUi.GetComponent<Animator>().Play("LevelSelectAway");
            bLevelSelect = false;
        }

        if (PauseMenuUi.activeInHierarchy)
        {
            PauseMenuUi.SetActive(false);
            UnPause();
            return;
        }


        if (bEndGame)
        {
            EndGameOpen();
            return;
        }

        PauseMenuUi.SetActive(true);
        Pause();


    }

    public void Pause() 
    {
        Time.timeScale = 0;
        bPaused = true;
    }
    public void UnPause() 
    {
        Time.timeScale = 1;
        bPaused = false;
    }

    public void OnAnyButton()
    {
        Speaker.PlayOneShot(ClickSound);
    }

    #endregion

    #region DragShoot
    private Vector2 OnScreentoCanvasPos(Vector2 OnScreenPos)
    {

        Vector2 CanvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            OnScreenPos,
            canvas.worldCamera,
            out CanvasPosition
            );

        return CanvasPosition;
    }


    public void OnBallDragStart(BaseEventData EventData) { BallInterface.OnBallGrabbed(); PlayUiAnim("BallGrabGo",2); /* Debug.LogWarning("BallDragStart");*/ }

    public void OnBallDrag(BaseEventData EventData)
    {
        PointerEventData pointerData = (PointerEventData)EventData;

        BallGrabImage.transform.position = canvas.transform.TransformPoint(OnScreentoCanvasPos(pointerData.position));

        PullBar.position = BallGrabImage.transform.position;
        
        RotateBars();



        Color32 newCol = LerpColor();

        BallInterface.HitMagnitudeToColor(newCol);
        ColorBars(newCol);


    }

    public void OnBallButtonDown(BaseEventData EventData) 
    {

       BallInterface.OnBallGrabbed();
       BallInterface.ActivateTrajectory(true);

    }
    public void OnBallButtonUp(BaseEventData EventData) { BallInterface.OnBallReleased(); BallInterface.ActivateTrajectory(false); }



    public void OnBallDragEnded(BaseEventData EventData) 
    {
        BallInterface.OnBallReleased();
        AdjustBallUiPos(StartGrabLocation);

        HitbarImage.color = new Color32(0, 0, 0, 0);
        PullbarImage.color = new Color32(0, 0, 0, 0);
    }


    public void AdjustBallUiPos(Vector2 OnScreenPos) { BallGrabImage.transform.position = canvas.transform.TransformPoint(OnScreenPos); }
    #endregion

    #region IMiniGolf

    public void OnBallReady() { BallInterface.OnBallReady();  PlayUiAnim("BallGrabCome", 2); }


    public void LevelStart() { PlayUiAnim("LevelLoadAnim"); }
    #endregion


    private void RotateBars() 
    {

        float angle = -Mathf.Atan2(HitBar.anchoredPosition.x - PullBar.anchoredPosition.x, HitBar.anchoredPosition.y - PullBar.anchoredPosition.y) * Mathf.Rad2Deg;
        HitBar.rotation = Quaternion.Euler(0, 0, angle);
        PullBar.rotation = Quaternion.Euler(0, 0, angle);   

    }

    private void ColorBars(Color32 NewColor) 
    {
        HitbarImage.color = NewColor;
        PullbarImage.color = NewColor;
    }

    private Color32 LerpColor() 
    {
        float Distance = Vector2.Distance(HitBar.anchoredPosition, PullBar.anchoredPosition);

        //Debug.Log(Distance.ToString());
        Distance = Distance / 350;
        byte Red = (byte)Mathf.Lerp(55, 255, Distance);
        byte Green = (byte)Mathf.Lerp(200, 25, Distance / 2);
        byte Blue = (byte)Mathf.Lerp(255, 55, Distance);
        //byte Alpha = (byte)Mathf.Lerp(0, 255, Distance);

        return new Color32(Red, Green, Blue, 255);
    }
}
