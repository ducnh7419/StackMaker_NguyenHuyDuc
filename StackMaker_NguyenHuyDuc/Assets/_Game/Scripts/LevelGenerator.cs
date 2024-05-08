using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;


public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private GameObject backgroundBlock;
    [SerializeField] private GameObject brickBlock;
    [SerializeField] private GameObject wallBlock;
    [SerializeField] private GameObject bridgeBlock;
    [SerializeField] private GameObject finishLineBlock;
    [SerializeField] private GameObject winPos;
    [SerializeField] private GameObject player;
    private new GameObject gameObject;
    private int noBrickBlock=0;
    public enum MapStructEnum{
        None,
        BrickBlock,
        WallBlock,
        BridegeBlock,
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
                            Vector3 playerPosition=startPoint+new Vector3(0,-0.1f,0);
                            if(playerDirection.x==0){
                                GameObject playerGo=Instantiate(player,playerPosition,Quaternion.Euler(playerDirection));
                                camera.GetComponent<CameraFollow>().target = playerGo.transform;
                            }else{
                                GameObject playerGo=Instantiate(player,playerPosition,player.transform.rotation);
                                camera.GetComponent<CameraFollow>().target = playerGo.transform;
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
                        noBrickBlock--;
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
        GameObject blockObject=Instantiate(gameOb,pos,gameOb.transform.rotation);
        blockObject.transform.SetParent(gameObject.transform);       
        if(gameOb!=bridgeBlock){
            for(int k=0;k<noBackgroundBlk;k++ ){
                pos.y+=-gameOb.transform.localScale.y;
                blockObject=Instantiate(backgroundBlock,pos,backgroundBlock.transform.rotation);
                blockObject.transform.SetParent(gameObject.transform);
            }
        }
        pos.x += gameOb.transform.localScale.x;
        pos.y=0;
    }

    private void FinishBridgeInstantiate(Vector3 pos){
        Vector3 startPoint=pos;
        for(int i=0;i<noBrickBlock;i++){
            GameObject finishBridge= new GameObject("Finish Bridge");
            finishBridge.gameObject.tag="Finish Bridge";
            GameObject blockObject=Instantiate(bridgeBlock,pos,brickBlock.transform.rotation);
            blockObject.gameObject.tag="Finish Bridge";
            blockObject.transform.SetParent(finishBridge.transform);
            pos.z-=brickBlock.transform.localScale.z;
            blockObject=Instantiate(wallBlock,pos,brickBlock.transform.rotation);
            blockObject.transform.SetParent(finishBridge.transform);
            pos.z+=brickBlock.transform.localScale.z*2;
            blockObject=Instantiate(wallBlock,pos,brickBlock.transform.rotation);
            blockObject.transform.SetParent(finishBridge.transform);
            pos.z-=brickBlock.transform.localScale.z;
            pos.x+=brickBlock.transform.localScale.x;
            finishBridge.transform.SetParent(gameObject.transform);
        }
        Vector3 direction=(pos-startPoint).normalized;
        pos.y-=3;
        Debug.Log(direction);
        if(!(Vector3.Distance(direction,Vector3.down)<=0.01f)){
            GameObject go=Instantiate(winPos,pos,Quaternion.Euler(new Vector3(0,90,0)));
            go.transform.SetParent(gameObject.transform);
        }else{
            GameObject go=Instantiate(winPos,pos,winPos.transform.rotation);
            go.transform.SetParent(gameObject.transform);
        }
        
    }

    private void Start() {
        gameObject=new GameObject("Map01");
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
