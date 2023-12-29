using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class IngameUI : MonoBehaviour
{
    [SerializeField] private GameObject openMenuButton;
    [SerializeField] private GameObject ingameMenu;
    [SerializeField] private CollectiblesList collectibles;
    [SerializeField] private TextMeshProUGUI collectibleText;
    [SerializeField] private TextMeshProUGUI textMessage;
    
    private void Start()
    {
        GameStateManager.instance.onStateChanged += OnStateChange;
        gameObject.SetActive(false);
    }


    private void OnStateChange(GameStateManager.GameState newState)
    {
        //we toggle the availability of the inGame menu whenever the game state changes
        bool isInGame = newState == GameStateManager.GameState.InGame;
        gameObject.SetActive(isInGame);
    }

    //this is called via the "got o main menu" button
    public void GoToMainMenu()
    {
        GameStateManager.instance.GoToMainMenu();
        CloseIngameUI();
    }

    //this is called via the "save game" button
    public void SaveGame()
    {
        GameStateManager.instance.SaveGame("SaveGame");
    }

    //this is called via the button in the upper left corner
    public void OpenIngameUI()
    {
        ingameMenu.SetActive(true);
        openMenuButton.SetActive(false);
        DisplayCollectedLetters();
    }

    private void DisplayCollectedLetters()
    {
        //we go through all collected collectibles,
        //get the scriptable object that belongs to that collectible
        //and append the name and content to a string
        //that we then show in the ingame menu. 
        string text = "";
        var collectedCollectibles = GameStateManager.instance.data.collectedCollectiblesIdentifiers;
        for (int index = 0; index < collectedCollectibles.Count; index++)
        {
            var letter = collectibles.GetDataByIdentifier(collectedCollectibles[index]);
            if (letter == null)
                return;
            text += letter.identifier.ToUpper() + "\n";
            text += letter.content + "\n\n";
        }

        StartCoroutine(LetterByLetterTextCoroutine(collectibleText, text));
    }

    private IEnumerator LetterByLetterTextCoroutine(TextMeshProUGUI textField, string text)
    {
        //this is a very basic implementation for displaying a text letter by letter.
        string currentText = "";
        for (int index = 0; index < text.Length; index++)
        {
            currentText += text[index];
            textField.text = currentText;
            yield return new WaitForSeconds(0.05f);
        }
    }

    //this is called via the button in the upper left corner, and by the GameStateManager.
    public void CloseIngameUI()
    {
        ingameMenu.SetActive(false);
        openMenuButton.SetActive(true);
    }
    
    //with this function we can show a small message at the bottom of the screen
    public void ShowMessage(string pressSpaceToSwitchScene)
    {
        textMessage.text = pressSpaceToSwitchScene;
    }

    //we can clear the text at the bottom of the screen again
    public void ClearMessage()
    {
        textMessage.text = "";
    }
}
