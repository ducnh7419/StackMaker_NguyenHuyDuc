using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelSelectionUI;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject pauseMenu;
    private static UIManager ins;
    public static UIManager Ins =>ins;

    // Update is called once per frame
    void Awake()
    {
        ins=this;
    }

    public void DeactiveAllUI(){
        startScreen.SetActive(false);
        mainMenu.SetActive(false);
        levelSelectionUI.SetActive(false);
        inGameUI.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void ShowStartScreen()
    {
        DeactiveAllUI();
        startScreen.SetActive(true);
    }

    public void ShowMainMenuUI()
    {
        DeactiveAllUI();
        mainMenu.SetActive(true);
    }

    public void ShowStartGameUI()
    {
        DeactiveAllUI();
        mainMenu.SetActive(false);
    }

    public void ShowPauseMenu(){
        showIngameUI();
        pauseMenu.SetActive(true);
    }

    public void showIngameUI(){
        DeactiveAllUI();
        inGameUI.SetActive(true);
    }

    internal void ShowLevelSelectionUI()
    {
        DeactiveAllUI();
        levelSelectionUI.SetActive(true);
    }
}
