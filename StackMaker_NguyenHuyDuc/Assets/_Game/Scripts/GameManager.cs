using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private static GameManager ins;
    public static GameManager Ins =>ins;

    public enum State{
        None=0,
        StartScreen=1,
        MainMenu=2,
        LevelSelection=3,
        StartGame=4,
        OnGame=5,
        PauseGame=6,
        EndGame=7
    }

    public State currState;

  
    // Update is called once per frame
    void Awake()
    {
        ins=this;
    }

    void Start(){
        currState=State.StartScreen;
        OnStartScreeen();
    }
    
    public void ChangeState(State state) {
        switch (state)
        {
            case State.None:
                break;
            case State.StartScreen:
                SaveState(State.StartScreen);
                OnStartScreeen();
                break;
            case State.MainMenu:
                SaveState(State.MainMenu);
                OnMainMenu();
                break;
            case State.LevelSelection:
                SaveState(State.LevelSelection);
                OnLevelSelection();
                break;
            case State.StartGame:
                SaveState(State.StartGame);
                OnStartGame();
                break;
            case State.OnGame:
                SaveState(State.OnGame);
                OnGame();
                break;
            case State.PauseGame:
                SaveState(State.PauseGame);
                OnPauseGame();
                break;
            case State.EndGame:
                SaveState(State.EndGame);
                OnEndGame();
                break;
        }
    }

    public void SaveState(State state){
        currState=state;
    }

    public void GoBackward(){
        ChangeState(--currState);
    }

    private void OnGame()
    {
        UIManager.Ins.showIngameUI();
    }

    private void OnPauseGame()
    {
        UIManager.Ins.ShowPauseMenu();       
    }

    private void OnStartGame()
    {
        UIManager.Ins.ShowStartGameUI();
        LevelManager.Ins.GenerateLevel();
        ChangeState(State.OnGame);

    }

    private void OnLevelSelection()
    {
        UIManager.Ins.ShowLevelSelectionUI();

    }

    private void OnStartScreeen()
    {
        UIManager.Ins.ShowStartScreen();
    }

    public void OnEndGame()
    {
        // logic end game
        Debug.Log("AAAA");
    }

    public void OnMainMenu() {
        
        UIManager.Ins.ShowMainMenuUI();
    }

}
