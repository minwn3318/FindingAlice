﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneChange : MonoBehaviour
{   
    static public bool checkLoad = false;
    
    public void NewGame(){
        if(DataController.Instance._gameData.playerPosition != new Vector3(0,0,0))
        {
            Debug.Log("데이터 소멸 경고");
            SceneManager.LoadScene("GameScene");
        }
        else
        SceneManager.LoadScene("GameScene");
    }

    public void LoadGame(){

            checkLoad = true;
            SceneManager.LoadScene("GameScene");

    }
}
