using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectionActionHandler : MonoBehaviour
{
    [SerializeField] List<Button> buttons = new List<Button>();
    [SerializeField] Button jumpBackwardButton;

    private void Start()
    {
        for(int i = 0; i < buttons.Count;i++){
            buttons[i].onClick.AddListener(OnClicked);
        }
        jumpBackwardButton.onClick.AddListener(()=>GameManager.Ins.GoBackward());
    }

    private void OnClicked(){
        string clickedButtonName=EventSystem.current.currentSelectedGameObject.name;
        UserDataManager.SaveGame(Convert.ToInt32(clickedButtonName));
        GameManager.Ins.ChangeState(GameManager.State.StartGame);   
    }
    
}
