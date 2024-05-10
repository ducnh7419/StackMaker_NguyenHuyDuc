using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{ 
    private static int currLevel=0;

    private static void SaveGame(){
        currLevel++;
        PlayerPrefs.SetInt("currLevel", currLevel);
    }

    public static int LoadGame(){
        return PlayerPrefs.GetInt("currLevel");
    }

    public static void LoadGame(int level){
        currLevel=level;
    }
}
