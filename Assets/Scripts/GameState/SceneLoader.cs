using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The SceneLoader unloads and loads scenes.
/// It has always one current scene that will be automatically unloaded when a new scene is to be loaded
/// The ManagerScene, however, will never be unloaded.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    private string currentScene; //this saves whatever the current loaded main scene is (Level1, Level2, or MainMenu)

    private void Awake()
    {
        instance = this;
    }

    public void SwitchScene(string newScene)
    {
        //when a scene is to be switched, we start a coroutine, so that we can first unload the current scene,
        //then load the next scene, while being able to do it asynchronous (here we could, for example, show a loading
        //screen animation)
        StartCoroutine(LoadNewSceneCoroutine(newScene));
    }
    
    private IEnumerator LoadNewSceneCoroutine(string newSceneName)
    {
        //first, show the loading screen, so that the player does not have to see elements plopping in and out of the scene
        UIManager.instance.ToggleLoadingScreen(true);
        
        //if the current scene is actually loaded, we first unload it
        var scene = SceneManager.GetSceneByName(currentScene);
        if (scene.isLoaded)
        {
            //by yielding for loading or unloading a scene, we can wait until the loading process is actually finished
            yield return SceneManager.UnloadSceneAsync(currentScene);
        }

        //then, when the scene we want to load is not yet loaded, we load it
        Scene newScene = SceneManager.GetSceneByName(newSceneName);
        if (!newScene.isLoaded)
        {
            yield return SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);
        }

        //all instantiated objects get added to the active scene.
        //this way, all bullets and VFX elements we instantiate will be cleaned up
        //when we unload the scene again later on.
        yield return null;
        newScene = SceneManager.GetSceneByName(newSceneName);
        SceneManager.SetActiveScene(newScene);
        
        //lastly, we disalbe the loading screen and set the current scene accordingly
        UIManager.instance.ToggleLoadingScreen(false);
        currentScene = newSceneName;
    }
}
