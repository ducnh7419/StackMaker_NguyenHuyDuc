using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    readonly LevelGenerator levelGenerator;
    public static LevelManager ins;
    public LevelManager Ins =>ins;



    // Update is called once per frame
    void Awake()
    {
        ins=this;
    }

    public void GenerateLevel(){
        levelGenerator.GenerateLevel(1);
    }
}
