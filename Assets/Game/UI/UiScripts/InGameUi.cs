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


    /// <summary>
    /// Clears possible paused state
    /// Gets PlayerBall
    /// Gets PlayerCamera
    /// SetsUp Ui references
    /// </summary>
    private void Start()
    {
        PauseMenuUi.SetActive(false);
        UnPause();
        GetBallInterface();

        GetCameraInterface();

        HitbarImage = HitBar.gameObject.GetComponent<Image>();
        PullbarImage = PullBar.gameObject.GetComponent<Image>();

        //LevelSelectUi.GetComponent<Animator>().Play("LevelSelectAway");

    }

    /// <summary>
    /// Check if buttons clicked
    /// </summary>
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
    /// <summary>
    /// Gets KnownBall
    /// </summary>
    /// <returns></returns>
    private bool GetBallInterface() { if (Ball.TryGetComponent(out IMiniGolf _BallInterface)) { BallInterface = _BallInterface; return true; } return false; }
    /// <summary>
    /// Gets MainCamera
    /// </summary>
    /// <returns></returns>
    private bool GetCameraInterface() { if (CameraController.TryGetComponent(out IMiniGolf _CameraInterface))  { CameraInterface = _CameraInterface; return true; }  return false; }
    /// <summary>
    /// Gets if Ui is ready to exit
    /// </summary>
    /// <returns></returns>
    public bool GetExitReady() { return bExitReady; }
    /// <summary>
    /// Sets Ui exit status
    /// </summary>
    public void EndGame() { bExitReady = true; }

    #endregion

    #region UIManagement
    /// <summary>
    /// Sends command to CameraManager through Ui to play given player camera anims
    /// Not Used by UiItself rather streamlines process through game objects
    /// </summary>
    /// <param name="AnimName"></param>
    public void PlayCamAnim(string AnimName) { if (CameraInterface != null || GetCameraInterface() ) { CameraInterface.PlayCamAnim(AnimName); }  }
    /// <summary>
    /// Sends command to CameraManager through Ui to play given player camera anim on given animation layer
    /// Not Used by UiItself rather streamlines process through game objects
    /// </summary>
    /// <param name="AnimName"></param>
    /// <param name="Layer"></param>
    public void PlayCamAnim(string AnimName, int Layer) { if (CameraInterface != null || GetCameraInterface() ) { CameraInterface.PlayCamAnim(AnimName,Layer); }  }

    /// <summary>
    /// Plays given Ui Animation
    /// </summary>
    /// <param name="AnimName"></param>
    public void PlayUiAnim(string AnimName) { if (AnimController) { AnimController.Play(AnimName); } }
    /// <summary>
    /// Plays given Ui Anim on given AnimLayer
    /// </summary>
    /// <param name="AnimName"></param>
    /// <param name="Layer"></param>
    public void PlayUiAnim(string AnimName, int Layer) { if (AnimController) { AnimController.Play(AnimName, Layer); } }

    /// <summary>
    /// Opens EndGame Ui
    /// </summary>
    public void EndGameOpen() { AnimController.Play("EndGameUiComeAnim", 1); bEndGame = true; } 
    /// <summary>
    /// Closes EndGameUi
    /// </summary>
    public void EndGameClose() { AnimController.Play("EndGameUiGoAnim", 1); }

    /// <summary>
    /// Updates PlayerScore on screen
    /// </summary>
    /// <param name="HitCount"></param>
    public void UpdateScore(int HitCount) { ScoreText.text = HitCount.ToString(); }

    /// <summary>
    /// Updates ScoreText
    /// </summary>
    /// <param name="NewText"></param>
    public void UpdateText(string NewText) { ScoreText.text = NewText; }
 
    /// <summary>
    /// Sets score one last time
    /// Calculates and sets LevelPlayTime
    /// Opens EndGameUi
    /// </summary>
    /// <param name="HitCount"></param>
    public void OnGameEnded(int HitCount) 
    { 
        EndScoreText.text = EndScoreText.text + HitCount.ToString(); 
        EndTimeText.text = EndTimeText.text + (Time.timeSinceLevelLoad / 60).ToString("F2"); 
        EndGameOpen(); 
    }


    #endregion


    #region Buttons


    /// <summary>
    /// Unpause in case game was paused
    /// Loads scene 0 = MainMenu Level
    /// </summary>
    public void BTN_MainMenu() { UnPause(); SceneManager.LoadScene(0); }

    /// <summary>
    /// Unpause in case game was paused
    /// if LevelSelect is already open = Close LevelSelectMenu
    /// if LevelSelect is not open = Open LevelSelectMenu
    /// </summary>
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

    /// <summary>
    /// Unpause incase game was paused
    /// Check CurrentLevel index
    /// if LastLevel go to MainMenuLevel
    /// if notLastLevel go to NextLevel
    /// </summary>
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
    /// <summary>
    /// Sets game paused
    /// safe Pause button regarding what menu we are in.
    /// </summary>
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
    /// <summary>
    /// Set timescale 0.
    /// good for our use
    /// </summary>
    public void Pause() 
    {
        Time.timeScale = 0;
        bPaused = true;
    }

    /// <summary>
    /// Set timescale 1.
    /// good for our use
    /// </summary>
    public void UnPause() 
    {
        Time.timeScale = 1;
        bPaused = false;
    }
    /// <summary>
    /// PlayButtonSound
    /// Add Button effects here.
    /// </summary>
    public void OnAnyButton()
    {
        Speaker.PlayOneShot(ClickSound);
    }

    #endregion

    #region DragShoot
    /// <summary>
    /// Convert OnScreen Position to CanvasPosition for scaling issues
    /// </summary>
    /// <param name="OnScreenPos"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Notify ball of being dragged for calculations
    /// Send BallGrab Ui Element away
    /// </summary>
    /// <param name="EventData"></param>
    public void OnBallDragStart(BaseEventData EventData) { BallInterface.OnBallGrabbed(); PlayUiAnim("BallGrabGo",2); /* Debug.LogWarning("BallDragStart");*/ }


    /// <summary>
    /// Calculate drag strength
    /// Adjust Hit rotation indicators rotations
    /// Adjust Hit Magnitude indicator colors
    /// </summary>
    /// <param name="EventData"></param>
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
    /// <summary>
    /// Notify ball of grab
    /// Activate trajectory
    /// Trajectory is a 3D Ui Element bound to the Ball so we manage it through here. 
    /// </summary>
    /// <param name="EventData"></param>
    public void OnBallButtonDown(BaseEventData EventData) 
    {

       BallInterface.OnBallGrabbed();
       BallInterface.ActivateTrajectory(true);

    }
    /// <summary>
    /// Notify ball of release
    /// Deactivate trajectory
    /// Trajectory is a 3D Ui Element bound to the Ball so we manage it through here. 
    /// </summary>
    /// <param name="EventData"></param>
    public void OnBallButtonUp(BaseEventData EventData) { BallInterface.OnBallReleased(); BallInterface.ActivateTrajectory(false); }


    /// <summary>
    /// Release ball
    /// Reset BallGrabUi in place
    /// Reset HitMagnitude Colors
    /// </summary>
    /// <param name="EventData"></param>
    public void OnBallDragEnded(BaseEventData EventData) 
    {
        BallInterface.OnBallReleased();
        AdjustBallUiPos(StartGrabLocation);

        HitbarImage.color = new Color32(0, 0, 0, 0);
        PullbarImage.color = new Color32(0, 0, 0, 0);
    }

    /// <summary>
    /// Reset BallGrabUi in place
    /// </summary>
    /// <param name="OnScreenPos"></param>
    public void AdjustBallUiPos(Vector2 OnScreenPos) { BallGrabImage.transform.position = canvas.transform.TransformPoint(OnScreenPos); }
    #endregion

    #region IMiniGolf
    /// <summary>
    /// Called when ball is stopped
    /// Brings BallGrabUi back
    /// </summary>
    public void OnBallReady() { BallInterface.OnBallReady();  PlayUiAnim("BallGrabCome", 2); }

    /// <summary>
    /// Called when level is ready to go by GameManager
    /// Plays LevelLoadAnim
    /// </summary>
    public void LevelStart() { PlayUiAnim("LevelLoadAnim"); }
    #endregion

    /// <summary>
    /// Calculates Hit and pull Ui rotations from Current touch position to ball position
    /// </summary>
    private void RotateBars() 
    {

        float angle = -Mathf.Atan2(HitBar.anchoredPosition.x - PullBar.anchoredPosition.x, HitBar.anchoredPosition.y - PullBar.anchoredPosition.y) * Mathf.Rad2Deg;
        HitBar.rotation = Quaternion.Euler(0, 0, angle);
        PullBar.rotation = Quaternion.Euler(0, 0, angle);   

    }
    /// <summary>
    /// Sets Bar Colors
    /// </summary>
    /// <param name="NewColor"></param>
    private void ColorBars(Color32 NewColor) 
    {
        HitbarImage.color = NewColor;
        PullbarImage.color = NewColor;
    }
    /// <summary>
    /// Lerps Bar Colors according to current hit magnitude calculations
    /// </summary>
    /// <returns></returns>
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
