using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameMenuActionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button retryButton;
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button nextLevelButton;
    private bool isPaused=false;
    void Start()
    { 
        retryButton.onClick.AddListener(OnRetryClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
    }

    private void OnNextLevelButtonClicked(){
        Time.timeScale=1;
        UserDataManager.SaveGame();
        GameManager.Ins.ChangeState(GameManager.State.StartGame);
    }

    private void OnMainMenuClicked(){
        Time.timeScale=1;
        GameManager.Ins.ChangeState(GameManager.State.MainMenu);
    }
    private void OnRetryClicked(){
        GameManager.Ins.ChangeState(GameManager.State.StartGame);
        Time.timeScale=1;
    }
}
