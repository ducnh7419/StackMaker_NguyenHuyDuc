using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;


public class LevelGenerator : MonoBehaviour
{
    private new Camera camera;
    [SerializeField] private GameObject backGroundImage;
    [SerializeField] private GameObject backgroundBlock;
    [SerializeField] private GameObject brickBlock;
    [SerializeField] private GameObject wallBlock;
    [SerializeField] private GameObject bridgeBlock;
    [SerializeField] private GameObject finishLineBlock;
    [SerializeField] private GameObject winPos;
    [SerializeField] private GameObject player;
    private int level = 0;
    private new GameObject gameObject;

    private new GameObject finishLine;
    private int noBrickBlock = 0;

    public enum Direct
    {
        Forward,
        Back,
        Right,
        Left,
        None
    }

    private Vector3 firstBridgeBlock;

    public int Level { get => level; set => level = value; }

    public enum MapStructEnum
    {
        None = 0,
        BrickBlock = 1,
        WallBlock = 2,
        BridegeBlock = 3,
        FinishLineBlock = 4,
        StartFinishLine = 5,
        FinishBridge = 6,
        StartPoint = 10
    }

    public void GenerateLevel(int level)
    {
        string path = String.Format("Assets/_Game/Resources/Level/{0}.txt", level);
        int[][] levelStruct = ReadTextFile(path);
        if(levelStruct==null){
            UserDataManager.SaveGame(UserDataManager.LoadGame()-1);
            GameManager.Ins.ChangeState(GameManager.State.StartGame);
        }
        Vector3 startPoint = new Vector3(0, 0, 0);
        Vector3 pos = new Vector3(0, 0, 0);
        for (int i = 0; i < levelStruct.GetLength(0); i++)
        {
            pos.x = 0;
            for (int j = 0; j < levelStruct[i].Length; j++)
            {
                switch (levelStruct[i][j])
                {
                    case (int)MapStructEnum.None:
                        pos.x += backgroundBlock.transform.localScale.x;
                        break;
                    case (int)MapStructEnum.BrickBlock:
                        if (startPoint != Vector3.zero)
                        {
                            Vector3 playerDirection = (pos - startPoint).normalized * -90;
                            // move player to the bottom edge of block 
                            Vector3 playerPosition = startPoint + new Vector3(0, -0.1f, 0);
                            if (playerDirection.x == 0)
                            {
                                GameObject playerGo = Instantiate(player, playerPosition, Quaternion.Euler(playerDirection));
                                camera.GetComponent<CameraFollow>().target = playerGo.transform;
                                playerGo.tag = "Player";
                            }
                            else
                            {
                                GameObject playerGo = Instantiate(player, playerPosition, player.transform.rotation);
                                camera.GetComponent<CameraFollow>().target = playerGo.transform;
                                playerGo.tag = "Player";
                            }
                            startPoint = Vector3.zero;
                        }
                        MapStructRowInstantiate(brickBlock, ref pos);
                        noBrickBlock++;
                        break;
                    case (int)MapStructEnum.WallBlock:
                        MapStructRowInstantiate(wallBlock, ref pos);
                        break;
                    case (int)MapStructEnum.BridegeBlock:
                        MapStructRowInstantiate(bridgeBlock, ref pos);
                        noBrickBlock--;
                        break;
                    case (int)MapStructEnum.FinishLineBlock:
                        MapStructRowInstantiate(finishLineBlock, ref pos);
                        break;
                    //5
                    case (int)MapStructEnum.StartFinishLine:
                        if(firstBridgeBlock==Vector3.zero){
                            firstBridgeBlock=pos;
                            MapStructRowInstantiate(finishLineBlock, ref pos);                       
                        }
                        else{
                            FinishBridgeInstantiate(pos,firstBridgeBlock);
                            MapStructRowInstantiate(finishLineBlock, ref pos);    
                        }
                        break;
                    //6
                    case (int)MapStructEnum.FinishBridge:
                        if(firstBridgeBlock==Vector3.zero){
                            firstBridgeBlock=pos;
                            pos.x += backgroundBlock.transform.localScale.x;
                        }
                        else{
                            FinishBridgeInstantiate(firstBridgeBlock,pos);
                        }
                        break;
                    case (int)MapStructEnum.StartPoint:
                        startPoint = pos;
                        MapStructRowInstantiate(brickBlock, ref pos);
                        noBrickBlock++;
                        break;
                }
            }
            pos.z += -backgroundBlock.transform.localScale.z;

        }
    }

    // Generate each block by row
    private void MapStructRowInstantiate(GameObject gameOb, ref Vector3 pos)
    {
        //number of background block in each column

        int noBackgroundBlk = 5;
        GameObject blockObject = Instantiate(gameOb, pos, gameOb.transform.rotation);
        if(gameOb.CompareTag("FinishLine")){
            blockObject.transform.SetParent(finishLine.transform);
        }else{
            blockObject.transform.SetParent(gameObject.transform);
        }
        if (gameOb != bridgeBlock)
        {
            for (int k = 0; k < noBackgroundBlk; k++)
            {
                pos.y += -gameOb.transform.localScale.y;
                blockObject = Instantiate(backgroundBlock, pos, backgroundBlock.transform.rotation);
                blockObject.transform.SetParent(gameObject.transform);
            }
        }
        pos.x += gameOb.transform.localScale.x;
        pos.y = 0;
    }

    private Direct CalculateBridgeDirection(Vector3 direction)
    {
        Direct direct = Direct.None;
        float angle1=Vector3.Angle(direction,Vector3.right);
        // Calculate the angle between direction vector and Vector Left(-1,0,0)
        float angle2=Vector3.Angle(direction,Vector3.left);
        if(angle1>=0f&&angle1<=90f){
            if(direction.y>=0){
                direct=angle1>=45f?Direct.Forward:Direct.Right;
            }else{
                direct=angle1>=45f?Direct.Back:Direct.Right;
            }
        }
        if(angle2>=0f&&angle2<=90f){
            if(direction.y>=0){
                direct=angle2>=45f?Direct.Forward:Direct.Left;
            }else{
                direct=angle2>=45f?Direct.Back:Direct.Left;
            }
        }
        return direct;
    }

    private void FinishBridgeInstantiate(Vector3 pos1,  Vector3 pos2)
    {       
        Direct direct = CalculateBridgeDirection((pos2-pos1).normalized);
        for (int i = 0; i < noBrickBlock - 1; i++)
        {
            GameObject finishBridge = new GameObject("Finish Bridge");
            finishBridge.tag = "Finish Bridge";
            GameObject blockObject = Instantiate(bridgeBlock, pos2, brickBlock.transform.rotation);
            blockObject.tag = "Finish Bridge";
            blockObject.transform.SetParent(finishBridge.transform);
            switch (direct)
            {
                case Direct.Right:
                    pos2.z -= brickBlock.transform.localScale.z;
                    break;
                case Direct.Left:
                    pos2.z -= brickBlock.transform.localScale.z;
                    break;
                case Direct.Back:
                    pos2.x -= brickBlock.transform.localScale.x;
                    break;
                case Direct.Forward:
                    pos2.x -= brickBlock.transform.localScale.x;
                    break;
            }
            blockObject = Instantiate(wallBlock, pos2, brickBlock.transform.rotation);
            blockObject.transform.SetParent(finishBridge.transform);
            switch (direct)
            {
                case Direct.Right:
                    pos2.z += brickBlock.transform.localScale.z * 2;
                    break;
                case Direct.Left:
                    pos2.z += brickBlock.transform.localScale.z * 2;
                    break;
                case Direct.Back:
                    pos2.x += brickBlock.transform.localScale.x * 2;
                    break;
                case Direct.Forward:
                    pos2.x += brickBlock.transform.localScale.x * 2;
                    break;
            }

            blockObject = Instantiate(wallBlock, pos2, brickBlock.transform.rotation);
            blockObject.transform.SetParent(finishBridge.transform);
            switch (direct)
            {
                case Direct.Right:
                    pos2.z -= brickBlock.transform.localScale.z;
                    pos2.x += brickBlock.transform.localScale.x;
                    break;
                case Direct.Left:
                    pos2.z -= brickBlock.transform.localScale.z;
                    pos2.x -= brickBlock.transform.localScale.x;
                    break;
                case Direct.Back:
                    pos2.x -= brickBlock.transform.localScale.x;
                    pos2.z += brickBlock.transform.localScale.z;
                    break;
                case Direct.Forward:
                    pos2.x -= brickBlock.transform.localScale.x;
                    pos2.z -= brickBlock.transform.localScale.z;
                    break;
            }

            finishBridge.transform.SetParent(gameObject.transform);
        }
        pos2.y -= 3;
        Vector3 direction = Vector3.zero;
        Quaternion rotation = winPos.transform.rotation;
        switch (direct)
        {
            case Direct.None:
                break;
            case Direct.Left:
                direction = Vector3.left;
                rotation = Quaternion.Euler(Vector3.up * -90);
                break;
            case Direct.Right:
                direction = Vector3.right;
                rotation = Quaternion.Euler(Vector3.up * 90);
                break;
            case Direct.Back:
                direction = Vector3.forward;
                rotation = winPos.transform.rotation;
                break;
            case Direct.Forward:
                direction = Vector3.back;
                rotation = Quaternion.Euler(Vector3.up * 180);
                break;

        }
        GameObject go = Instantiate(winPos, pos2, rotation);
        go.transform.SetParent(gameObject.transform);

    }

    private void OnInit()
    {
        noBrickBlock = 0;
        firstBridgeBlock=Vector3.zero;
    }

    private void OnEnable()
    {
        OnInit();
        if (level != 0)
        {
            camera = Camera.main;
            GameObject bg = Instantiate(backGroundImage, backGroundImage.transform.position, backGroundImage.transform.rotation);
            gameObject = new GameObject("Map" + level);
            finishLine= new GameObject("Finish Line");           
            gameObject.tag = "Map";
            finishLine.transform.SetParent(gameObject.transform);
            bg.transform.SetParent(gameObject.transform);
            bg.transform.GetChild(0).GetComponent<Canvas>().worldCamera = camera;
            GenerateLevel(level);
        }
    }

    private void OnDisable()
    {
        Destroy(gameObject);
        Destroy(GameObject.FindGameObjectWithTag("Player"));
    }

    public int[][] ReadTextFile(string filePath)
    {

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            int[][] mapStruct = new int[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                // Feature of each block     
                string[] blockFeatures = lines[i].Trim().Split();
                int[] blockFeaturesConverted = new int[blockFeatures.Length];
                for (int j = 0; j < blockFeatures.Length; j++)
                {
                    blockFeaturesConverted[j] = Convert.ToInt32(blockFeatures[j]);
                }
                mapStruct[i] = blockFeaturesConverted;
            }
            return mapStruct;
        }
        else
        {           
            Debug.LogError("File not found at path: " + filePath);
            return null;
        }
        
    }
}
