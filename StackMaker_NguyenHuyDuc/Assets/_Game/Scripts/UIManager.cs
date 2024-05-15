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
    [SerializeField] private GameObject endGameUI;    
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Animator transitionAnim;
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
        endGameUI.SetActive(false);
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
        ShowIngameUI();
        pauseMenu.SetActive(true);
    }

    public void ShowIngameUI(){
        DeactiveAllUI();
        RunTransition();
        inGameUI.SetActive(true);
    }

    public void ShowLevelSelectionUI()
    {
        DeactiveAllUI();
        levelSelectionUI.SetActive(true);
    }

    public void RunTransition(){
        transitionAnim.gameObject.SetActive(true);
        StartCoroutine(SceneTransition());
    }

    IEnumerator SceneTransition(){
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(2);
        transitionAnim.SetTrigger("start");
        transitionAnim.gameObject.SetActive(false);
    }

    public void ShowEndGameUI()
    {
        endGameUI.SetActive(true);
    }
}
