using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUi : MonoBehaviour
{


    public void OnNewGame() 
    {
        SceneManager.LoadScene("1");
    }

    public void OnLevelSelected(int LevelId) 
    {


        SceneManager.LoadScene(LevelId);
    }

}
