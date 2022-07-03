using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUi : MonoBehaviour
{

    [SerializeField]
    private AudioSource Speaker;

    [SerializeField]
    private AudioClip ClickSound;

    public void OnLevelSelected(string LevelName) 
    {
        //Sc_GameInstance.GameInstance.StartLevelLoad(LevelName);

        SceneManager.LoadScene(LevelName);    
    }

    public void OnAnyButton()
    {
        Speaker.PlayOneShot(ClickSound);
    }


}
