using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameMenuActionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button retryButton;
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button nextLevelButton;
    [SerializeField] TextMeshProUGUI score;
    void Start()
    { 
        retryButton.onClick.AddListener(OnRetryClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);     
        score.fontSize = 150;
    }

    private void Update() {
        score.SetText(UserDataManager.LoadScore().ToString());
    }

    private void OnNextLevelButtonClicked(){
        Time.timeScale=1;
        GameManager.Ins.ChangeState(GameManager.State.StartGame);
    }

    private void OnMainMenuClicked(){
        Time.timeScale=1;
        GameManager.Ins.ChangeState(GameManager.State.MainMenu);
    }
    private void OnRetryClicked(){
        UserDataManager.SaveGame(UserDataManager.LoadGame()-1);
        GameManager.Ins.ChangeState(GameManager.State.StartGame);
        Time.timeScale=1;
    }
}
