using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuActionHandler : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button levelSelectionButton;
    

    void Start()
    {
        playButton.onClick.AddListener(()=>GameManager.Ins.ChangeState(GameManager.State.StartGame));
        levelSelectionButton.onClick.AddListener(()=>GameManager.Ins.ChangeState(GameManager.State.LevelSelection));
    }
}
