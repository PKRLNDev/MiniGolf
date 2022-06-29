using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sc_GameMode : MonoBehaviour, IMiniGolf
{

    #region Variables

    [SerializeField]
    private GameObject GO_Ui;


    private IMiniGolf UiInterface;



    #endregion


    #region GetFunctions

    /// <summary>
    /// Gets Camera Interface from CameraObjectInput
    /// </summary>
    /// <returns>true if CameraInterface is valid</returns>
    private bool GetCameraInterface() 
    { 
        if (GO_Ui.TryGetComponent(out IMiniGolf _UiInterface)) 
        { 
            UiInterface = _UiInterface; 
            return true; } 
        return false; 
    }

    /// <summary>
    /// Finds all objects with tag UnSunkBall
    /// </summary>
    /// <returns>UnSunkBall Count</returns>
    public int GetActiveBallCount()
    {
        if (GameObject.FindGameObjectsWithTag("UnSunkBall") != null)
        {
            return GameObject.FindGameObjectsWithTag("UnSunkBall").Length;
        }

        return 0;
    }
    
    #endregion



    #region AsyncPrivateFunctions


    /// <summary>
    /// Async For Input
    /// </summary>
    /// <param name="Key"></param>
    /// <returns></returns>
    private IEnumerator WaitForEndGameReady()
    {
        bool done = false;
        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            if (UiInterface.GetExitReady())
            {
                done = true; // breaks the loop
                
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }
    }

    /// <summary>
    /// Async For EndGame
    /// </summary>
    /// <returns></returns>
    private IEnumerator EndGameCoRoutine(int HitCount)
    {
        

        yield return new WaitForSeconds(1.5f);

        
        UiInterface.OnGameEnded(HitCount);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }


    #endregion



    // Start is called before the first frame update
    void Start()
    {
        GetCameraInterface();
        Debug.LogWarning("Active ball Count = " + GetActiveBallCount().ToString());
    }


    #region PublicFunctions

    public void OnBallSunk(int HitCount)
    {

        //TODO AlsoGet BallHitCount, BallSinkTime, BallPlayerID.
        if (GetActiveBallCount() == 0)
        {

            UiInterface.UpdateText("CONGRATULATIONS.\n YOU WON! \n");

            StartCoroutine(EndGameCoRoutine(HitCount));
        }
    }

    /*
    public void OnBallSunk(int HitCount, IMiniGolf Ball)
    {

        //TODO AlsoGet BallHitCount, BallSinkTime, BallPlayerID.
        if (GetActiveBallCount() == 0)
        {
            UiInterface.UpdateText("CONGRATULATIONS.\n YOU WON! \n");

            StartCoroutine(EndGameCoRoutine());
        }
    }
    */



    public void UpdateScore(int HitCount)
    {
        UiInterface.UpdateScore(HitCount);
    }

    #endregion

}
