using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager ins;
    public static LevelManager Ins =>ins;
    [SerializeField] GameObject mapGenerator;
    private Camera m_Camera;



    // Update is called once per frame
    void Awake()
    {
        ins=this;
        m_Camera=Camera.main;
    }

    public void GenerateLevel(){
        mapGenerator.GetComponent<LevelGenerator>().Level=1;
        mapGenerator.SetActive(true);        
    }    
}
