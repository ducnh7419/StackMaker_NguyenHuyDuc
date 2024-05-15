using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager ins;
    public static LevelManager Ins =>ins;
    [SerializeField] GameObject mapGenerator;

    // Update is called once per frame
    void Awake()
    {
        ins=this;
    }

    public void GenerateLevel(){
        DestroyLevel();
        mapGenerator.GetComponent<LevelGenerator>().Level=UserDataManager.LoadGame();    
        mapGenerator.SetActive(true);
    }

    public void DestroyLevel(){
        mapGenerator.SetActive(false);
    }

}
