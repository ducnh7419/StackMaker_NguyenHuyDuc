using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject mainMenu;
    private static UIManager ins;
    public static UIManager Ins =>ins;

    public void ShowStartScreen()
    {
        startScreen.SetActive(true);
    }



    // Update is called once per frame
    void Awake()
    {
        ins=this;
    }

    public void ShowMainMenuUI()
    {
        startScreen.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ShowStartGameUI()
    {
        mainMenu.SetActive(false);
    }
}
