using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{ 

    public static void SaveGame(){
        PlayerPrefs.SetInt("currLevel", LoadGame()+1); 
    }

    public static void SaveScore(int score){
        PlayerPrefs.SetInt("score",score);
    }

    public static int LoadScore(){
       return PlayerPrefs.GetInt("score",0);
    }

    public static int LoadGame(){
        return PlayerPrefs.GetInt("currLevel",1);  

    }

    public static void SaveGame(int level){
        PlayerPrefs.SetInt("currLevel", level);
    }
}
