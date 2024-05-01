using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager ins;
    public GameManager Ins =>ins;



    // Update is called once per frame
    void Awake()
    {
        ins=this;
    }
}
