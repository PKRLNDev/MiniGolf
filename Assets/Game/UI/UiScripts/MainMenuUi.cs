using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUi : MonoBehaviour
{



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

}
