using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Sc_GameInstance : MonoBehaviour, IMiniGolf
{

    public static Sc_GameInstance GameInstance;

    [SerializeField]
    private Canvas LoadingUi;



    /// <summary>
    /// Keeps this class alive through levels
    /// destroys new spawns of this class
    /// </summary>
    private void Awake()
    {
        if (GameInstance == null)
        {
            GameInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
        LoadingUi.gameObject.SetActive(false);

    }



    public void StartLevelLoad(string LevelName) { StartCoroutine(LoadLevel(LevelName)); }

    private IEnumerator LoadLevel(string LevelName) 
    {


        var scene = SceneManager.LoadSceneAsync(LevelName);

        scene.allowSceneActivation = false;

        LoadingUi.gameObject.SetActive(true);

        do
        {
            yield return new WaitForSeconds(0.1f);
       
        } while (scene.progress < 0.9f);
        
        scene.allowSceneActivation = true;
        
        yield return new WaitForSeconds(0.5f);

        LoadingUi.gameObject.SetActive(false);
    }



    public bool GetLevelLoading(string LevelName) { return SceneManager.GetSceneByName(LevelName).isLoaded; }

}
