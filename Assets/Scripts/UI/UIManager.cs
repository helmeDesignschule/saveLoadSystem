using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    [SerializeField] private GameObject loadingScreen;
    public IngameUI ingameUI;


    private void Awake()
    {
        instance = this;
        loadingScreen.SetActive(false);
    }

    public void ToggleLoadingScreen(bool enabled)
    {
        loadingScreen.SetActive(enabled);
    }


}
