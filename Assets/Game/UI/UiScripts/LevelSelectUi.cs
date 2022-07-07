using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelSelectUi : MonoBehaviour
{
    [SerializeField]
    private Animator animMan;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MenuCome() { animMan.SetTrigger("Come"); }
    public void MenuComeToGo() { animMan.SetTrigger("ComeToGo"); }
    public void MenuGo() { animMan.SetTrigger("StayToGo"); }


    public void OnLevelSelected(string LevelName)
    {
        SceneManager.LoadScene(LevelName);
    }


    


}
