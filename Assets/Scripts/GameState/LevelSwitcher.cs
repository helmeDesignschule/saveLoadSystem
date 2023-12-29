
using UnityEngine;

/// <summary>
/// This is a simple test component we use to switch between two different scenes.
/// We do this to validate if data is correctly retained.
/// </summary>
public class LevelSwitcher : MonoBehaviour
{

    [SerializeField] private string sceneToLoad;
    private bool playerIsInside;
    void Update()
    {
        if (playerIsInside && Input.GetKeyDown(KeyCode.Space))
        {
            GameStateManager.instance.LoadNewGameplayScene(sceneToLoad);
        } 
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) 
            return;
        
        playerIsInside = true;
        UIManager.instance.ingameUI.ShowMessage("Press Space to switch scene");
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) 
            return;
        
        playerIsInside = false;
        UIManager.instance.ingameUI.ClearMessage();

    }
}
