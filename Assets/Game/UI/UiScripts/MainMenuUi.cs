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

    public GameObject LevelSelectGO;

    public bool bLevelSelect;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (bLevelSelect)
            {
                LevelSelectGO.GetComponent<Animator>().SetTrigger("StayToGo");
                gameObject.GetComponent<Animator>().SetTrigger("OnBackToMainMenu");
                bLevelSelect = true;
                return;
            }

        }


    }

    public void OnLevelSelected(string LevelName) 
    {
        //Sc_GameInstance.GameInstance.StartLevelLoad(LevelName);

        SceneManager.LoadScene(LevelName);    
    }

    public void BTN_Classic() 
    {
        if (bLevelSelect)
        {
            bLevelSelect = false;
            return;
        }
        bLevelSelect=true;
    }

    public void OnAnyButton()
    {
        Speaker.PlayOneShot(ClickSound);
    }


}
