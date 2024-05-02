using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager ins;
    public UIManager Ins =>ins;



    // Update is called once per frame
    void Awake()
    {
        ins=this;
    }
}
