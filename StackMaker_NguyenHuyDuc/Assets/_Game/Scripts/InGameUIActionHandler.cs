using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIActionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button pauseButton;
    [SerializeField] Button continueButton;
    [SerializeField] Button retryButton;
    [SerializeField] Button mainMenuButton;
    // [SerializeField] Button nextLevelButton;
    private bool isPaused=false;
    void Start()
    {
        pauseButton.onClick.AddListener(OnPauseContinueClicked);
        continueButton.onClick.AddListener(OnPauseContinueClicked); 
        retryButton.onClick.AddListener(OnRetryClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }


    private void OnMainMenuClicked(){
        Time.timeScale=1;
        GameManager.Ins.ChangeState(GameManager.State.MainMenu);
    }

    private void OnPauseContinueClicked(){
        Debug.Log("BBBBB");
        isPaused=!isPaused;
        Time.timeScale=isPaused?0:1;
        GameManager.Ins.ChangeState(isPaused?GameManager.State.PauseGame:GameManager.State.OnGame);
    }

    private void OnRetryClicked(){
        GameManager.Ins.ChangeState(GameManager.State.StartGame);
        Time.timeScale=1;
    }
    
}
