using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartNewGame()
    {
        GameStateManager.instance.StartNewGame();
    }

    public void LoadGame()
    {
        GameStateManager.instance.LoadFromSave("SaveGame");
    }
}
