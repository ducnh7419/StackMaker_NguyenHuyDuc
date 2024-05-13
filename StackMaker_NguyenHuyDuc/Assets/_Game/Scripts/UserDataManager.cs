using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{ 
    private static int currLevel=1;

    public static void SaveGame(){
        PlayerPrefs.SetInt("currLevel", LoadGame()+1);
    }

    public static int LoadGame(){
        currLevel=PlayerPrefs.GetInt("currLevel");  
        return currLevel;
    }

    public static void SaveGame(int level){
        currLevel=level;
        PlayerPrefs.SetInt("currLevel", currLevel);
    }
}
