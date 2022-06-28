using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Sc_GameMode : MonoBehaviour, IMiniGolf
{

    #region Variables

    [SerializeField]
    private TextMeshProUGUI ScoreText;

    #endregion


    #region AsyncPrivateFunctions


    /// <summary>
    /// Async For Input
    /// </summary>
    /// <param name="Key"></param>
    /// <returns></returns>
    private IEnumerator WaitForInput(KeyCode Key)
    {
        bool done = false;
        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            if (Input.GetKeyDown(Key))
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
    private IEnumerator EndGameCoRoutine()
    {
        

        yield return new WaitForSeconds(2.5f);

        yield return WaitForInput(KeyCode.Space);

        yield return null;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }


    #endregion


    // Start is called before the first frame update
    void Start()
    {
        Debug.LogWarning("Active ball Count = " + GetActiveBallCount().ToString());
    }

    #region DEPRECATED
    //// Update is called once per frame
    //void Update()
    //{

    //}

    //TODO ENDGAME
    //private void EndGame()
    //{
    //    ScoreText.text = "CONGRATULATIONS.\n YOU WON! \n" + ScoreText.text;

    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //} 
    #endregion

    #region PublicFunctions


    public void OnBallSunk(int HitCount)
    {
        Debug.LogWarning("YourScore = " + HitCount.ToString());

        //TODO AlsoGet BallHitCount, BallSinkTime, BallPlayerID.
        if (GetActiveBallCount() == 0)
        {
            ScoreText.text = "CONGRATULATIONS.\n YOU WON! \n" + ScoreText.text;
            StartCoroutine(EndGameCoRoutine());
        }
    }

    public int GetActiveBallCount()
    {
        if (GameObject.FindGameObjectsWithTag("UnSunkBall") != null)
        {
            return GameObject.FindGameObjectsWithTag("UnSunkBall").Length;
        }

        return 0;
    }

    public void UpdateScore(int HitCount)
    {
        ScoreText.text = "YourScore = " + HitCount.ToString();
    }

    #endregion

}
