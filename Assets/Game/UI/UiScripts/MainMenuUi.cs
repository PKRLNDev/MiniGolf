using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUi : MonoBehaviour
{

    [SerializeField]
    private AudioSource Speaker;

    [SerializeField]
    private AudioClip ClickSound;

    public void OnLevelSelected(string LevelName) 
    {

        if (Sc_GameInstance.GameInstance!=null)
        {

            if (!Sc_GameInstance.GameInstance.GetLevelLoading(LevelName))
            {

                Sc_GameInstance.GameInstance.StartLevelLoad(LevelName);
                
                return;
            }
            
        }

        Debug.Log("GAME INSTANCE IS NULL");
    }

    public void OnAnyButton()
    {
        Speaker.PlayOneShot(ClickSound);
    }


}
