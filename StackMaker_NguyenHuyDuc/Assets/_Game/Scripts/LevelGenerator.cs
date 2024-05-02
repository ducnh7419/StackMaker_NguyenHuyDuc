using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;


public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject backgroundBlock;
    [SerializeField] private GameObject brickBlock;
    [SerializeField] private GameObject wallBlock;
    [SerializeField] private GameObject bridgeBlock;
    [SerializeField] private GameObject bridgeWallBlock;
    [SerializeField] private GameObject finishLineBlock;
    [SerializeField] private GameObject player;
    private int noBrickBlock=0;
    public enum MapStructEnum{
        None,
        BrickBlock,
        WallBlock,
        BridegeBlock,
        BridgeWallBlock,
        FinishLineBlock,
        FinishBridge,
        StartPoint=10
    }

    public void GenerateLevel(int level){
        string path=String.Format("Assets/_Game/Resources/Level/{0}.txt",level);
        int[][] levelStruct=ReadTextFile(path);
        Vector3 startPoint=new Vector3(0,0,0);
        Vector3 pos=new Vector3(0,0,0);
        for (int i = 0; i < levelStruct.GetLength(0); i++)
        {
            pos.x=0;
            for (int j = 0; j < levelStruct[i].Length; j++)
            {
                switch(levelStruct[i][j]){
                    case (int)MapStructEnum.None:
                        pos.x+=backgroundBlock.transform.localScale.x;
                        break;
                    case (int)MapStructEnum.BrickBlock:
                        if(startPoint!= Vector3.zero){
                            Debug.Log(startPoint);
                            Vector3 playerDirection=(pos-startPoint).normalized*90;
                            // move player to the bottom edge of block 
                            Vector3 playerPosition=startPoint+new Vector3(-brickBlock.transform.localScale.x+0.1f,-0.15f,-brickBlock.transform.localScale.z*1.5f);
                            if(playerDirection.x==0){   
                                Instantiate(player,playerPosition,Quaternion.Euler(playerDirection));
                            }else{
                                Instantiate(player,playerPosition,player.transform.rotation);
                            }    
                            startPoint=Vector3.zero;
                        }              
                        MapStructRowInstantiate(brickBlock,ref pos);
                        noBrickBlock++;
                        break;
                    case (int)MapStructEnum.WallBlock:
                        MapStructRowInstantiate(wallBlock,ref pos);
                        break;
                    case (int)MapStructEnum.BridegeBlock:
                        MapStructRowInstantiate(bridgeBlock,ref pos);
                        break;
                    case (int)MapStructEnum.BridgeWallBlock:
                        MapStructRowInstantiate(bridgeWallBlock,ref pos);
                        break;
                    case (int)MapStructEnum.FinishLineBlock:
                        MapStructRowInstantiate(finishLineBlock,ref pos);
                        break;
                    case (int)MapStructEnum.FinishBridge:
                        FinishBridgeInstantiate(pos);
                        break;
                    case (int)MapStructEnum.StartPoint:
                        startPoint=pos;
                        MapStructRowInstantiate(brickBlock,ref pos);
                        noBrickBlock++;
                        break;                                          
                }
            }
            pos.z+=-backgroundBlock.transform.localScale.z;
            
        }
    }

    // Generate each block by row
    private void MapStructRowInstantiate(GameObject gameOb, ref Vector3 pos){
        //number of background block in each column
        
        int noBackgroundBlk=5;
        Instantiate(gameOb,pos,gameOb.transform.rotation);
        if(gameOb!=bridgeBlock&&gameOb!=bridgeWallBlock){    
            for(int k=0;k<noBackgroundBlk;k++ ){
                pos.y+=-gameOb.transform.localScale.y;
                Instantiate(backgroundBlock,pos,backgroundBlock.transform.rotation);
            }
        }
        pos.x += gameOb.transform.localScale.x;
        pos.y=0;
    }

    private void FinishBridgeInstantiate(Vector3 pos){
        for(int i=0;i<noBrickBlock;i++){
            Instantiate(bridgeBlock,pos,brickBlock.transform.rotation);
            pos.z-=brickBlock.transform.localScale.z;
            Instantiate(bridgeWallBlock,pos,brickBlock.transform.rotation);
            pos.z+=brickBlock.transform.localScale.z*2;
            Instantiate(bridgeWallBlock,pos,brickBlock.transform.rotation);
            pos.z-=brickBlock.transform.localScale.z;
            pos.x+=brickBlock.transform.localScale.x;
        }
    }

    private void Start() {
        GenerateLevel(1);
    }

    public int[][] ReadTextFile(string filePath)
    {
        
        if (File.Exists(filePath))
        {           
            string[] lines = File.ReadAllLines(filePath);
            int[][] mapStruct=new int[lines.Length][];
            for(int i=0;i<lines.Length;i++)
            {
                // Feature of each block     
                string[] blockFeatures=lines[i].Trim().Split();
                int[] blockFeaturesConverted=new int[blockFeatures.Length];               
                for(int j=0;j<blockFeatures.Length;j++){
                    blockFeaturesConverted[j]=Convert.ToInt32(blockFeatures[j]);
                }
                mapStruct[i]=blockFeaturesConverted;   
            }
            return mapStruct;
        }
        else
        {
            Debug.LogError("File not found at path: " + filePath);
        }
        return null;
    }
}
