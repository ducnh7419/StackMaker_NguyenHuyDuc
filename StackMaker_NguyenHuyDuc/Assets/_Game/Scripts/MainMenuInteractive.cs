using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuInteractive : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button levelSelectionButton;


    // Update is called once per frame
    void Update()
    {
        playButton.onClick.AddListener(()=>GameManager.Ins.ChangeState(GameManager.State.StartGame));
    }
}
