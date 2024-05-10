using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private static GameManager ins;
    public static GameManager Ins =>ins;
    

    public enum State{
        None=0,
        StartScreen=1,
        MainMenu=2,
        StartGame=3,
        EndGame=4
    }

  
    // Update is called once per frame
    void Awake()
    {
        ins=this;
        OnStartScreeen();
    }
    
    public void ChangeState(State state) {
        switch (state)
        {
            case State.None:
                break;
            case State.StartScreen:
                OnStartScreeen();
                break;
            case State.MainMenu:
                OnMainMenu();
                break;
            case State.StartGame:
                OnStartGame();
                break;
            case State.EndGame:
                OnEndGame();
                break;
        }
    }

    private void OnStartGame()
    {
        UIManager.Ins.ShowStartGameUI();
        LevelManager.Ins.GenerateLevel();

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
