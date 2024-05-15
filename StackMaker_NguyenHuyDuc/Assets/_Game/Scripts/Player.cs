using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator playerAnim;
    private Vector3 buttonDownPoint=Vector3.zero;
    private Vector3 buttonUpPoint=Vector3.zero;
    [SerializeField]private float speed;
    private bool isMoving;
    private int noBrick=0;
    private int score=0;
    private GameManager gameManager;

    public enum Direct{      
        Forward,
        Back,
        Right,
        Left,
        None
    }
    Direct direct;

    void Start(){
        OnInit();
    }

    void OnInit(){
        score=0;
        noBrick=0;
        StopMoving();
        buttonUpPoint=Vector3.zero;
        buttonDownPoint=Vector3.zero;
    }
    

    // Update is called once per frame
    private void Update()
    {
        PlayerControl();
    }

    private void StopMoving(){
        isMoving=false;
        direct=Direct.None;
        rb.velocity=Vector3.zero;
    }

    private void PlayerControl(){
        if(!isMoving){
            if(Input.GetMouseButtonDown(0)){
                buttonDownPoint=Input.mousePosition;
            }
            if(Input.GetMouseButtonUp(0)){
                buttonUpPoint=Input.mousePosition;
            }
        }
        if(Vector3.Distance(buttonDownPoint,buttonUpPoint)<0.0001f){
            
            OnInit();
            return;
        }
        Vector3 direction=Vector3.zero;
        if(buttonDownPoint!=Vector3.zero&&buttonUpPoint!=Vector3.zero){
            direction=(buttonUpPoint-buttonDownPoint).normalized;
            buttonUpPoint=Vector3.zero;
            buttonUpPoint=Vector3.zero;            
            // Calculate the angle between direction vector and Vector Right(1,0,0)
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
        }
        Vector3 directionVector=Vector3.zero;
        switch(direct){
                case Direct.Forward:                  
                    Debug.Log("Forward");
                    directionVector=Vector3.forward;
                    if(!CheckStopCondition(directionVector)){
                        isMoving=true;
                        AddOrRemoveBrick();
                        rb.velocity=speed * Time.deltaTime * directionVector;
                    }else{
                        StopMoving();
                    }
                    // rb.rotation=Quaternion.Euler(0,0,0);
                    rb.rotation=Quaternion.Euler(Vector3.up*180);
                    break;
                case Direct.Back:
                    directionVector=Vector3.back;
                    if(!CheckStopCondition(directionVector)){
                        isMoving=true;
                        AddOrRemoveBrick();
                        rb.velocity=speed * Time.deltaTime * directionVector;
                    }else{
                        StopMoving();
                    }
                    rb.rotation=Quaternion.Euler(0,0,0);
                    // rb.rotation=Quaternion.Euler(Vector3.down*180);
                    break;
                case Direct.Left:
                    directionVector=Vector3.left;
                    if(!CheckStopCondition(directionVector)){
                        isMoving=true;
                        AddOrRemoveBrick();
                        rb.velocity=speed * Time.deltaTime * directionVector;
                    }else{
                        StopMoving();
                    }           
                    // rb.rotation=Quaternion.Euler(Vector3.down*90);
                     rb.rotation=Quaternion.Euler(Vector3.up*90);
                    break;
                case Direct.Right:
                    directionVector=Vector3.right;
                    if(!CheckStopCondition(directionVector)){
                        isMoving=true;
                        AddOrRemoveBrick();
                        rb.velocity=speed * Time.deltaTime * directionVector;
                    }else{
                        StopMoving();
                    }
                    // rb.rotation=Quaternion.Euler(Vector3.up*90);
                    rb.rotation=Quaternion.Euler(Vector3.down*90);
                    break;
                case Direct.None:
                    StopMoving();
                    break;
        }
        ReachingChest(ref direct);
             
    }

    private void AddOrRemoveBrick(){
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f))
        {
            
            GameObject block=hit.collider.gameObject;
            if(block.CompareTag("brick")){
                AddBrick(rb.gameObject,block);
            }
            if(block.CompareTag("bridge")){
                RemoveBrick(rb.gameObject,block);                
            }
            if(block.CompareTag("Finish Bridge")){          
                block.transform.parent.GetChild(1).GetChild(0).GetComponent<Renderer>().material.color=Color.blue;
                block.transform.parent.GetChild(2).GetChild(0).GetComponent<Renderer>().material.color=Color.green;
                RemoveBrick(rb.gameObject,block);
                if(noBrick==0){
                    direct=Direct.None;
                    GameManager.Ins.ChangeState(GameManager.State.EndGame);
                }
            }
            block.GetComponent<Collider>().enabled=false;
        }
    }

    private void ReachingChest( ref Direct direct){
        Vector3 directionVector=Vector3.zero;
        switch(direct){
            case Direct.None:
                break;
            case Direct.Left:
                directionVector=Vector3.left;
                break;
            case Direct.Right:
                directionVector=Vector3.right;
                break;
            case Direct.Forward:
                directionVector=Vector3.forward;
                break;
            case Direct.Back:
                directionVector=Vector3.back;
                break;
        }
        if (Physics.Raycast(transform.position, directionVector , out RaycastHit hit, 1f)){
            if(hit.collider.gameObject.CompareTag("Chest")){
                GameObject closeChest=hit.collider.gameObject;
                GameObject openChess=closeChest.transform.parent.GetChild(closeChest.transform.parent.childCount-1).gameObject;
                openChess.SetActive(true);
                closeChest.SetActive(false);
                StopMoving();
                StartCoroutine(EndGame());            
            }
            
        }
    }

    IEnumerator EndGame(){
        Debug.Log(score);
        playerAnim.SetInteger("renwu", 2);
        UserDataManager.SaveGame();
        UserDataManager.SaveScore(score); 
        yield return new WaitForSeconds(6);     
        Time.timeScale=0;       
        GameManager.Ins.ChangeState(GameManager.State.EndGame);
    }

    //Stop player when reaching wall or  bridge when he don't have enought brick
    private bool CheckStopCondition(Vector3 direction){       
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, .5f))
        {
            GameObject block=hit.collider.gameObject;
            if(block.CompareTag("wall")){
                return true;
            }
            if(block.CompareTag("Finish Bridge")||block.CompareTag("bridge")){
                if(noBrick==0){
                    return true;
                }
            }
        }
        return false;
    }

    private void AddBrick(GameObject player,GameObject brickBlock){
        // Get Brick GameObject Transform of brick block
        Transform brick=brickBlock.transform.GetChild(0);
        //Get player animation
        GameObject playerAnim= player.transform.GetChild(0).gameObject;
        // find the real size of brick
        Vector3 size=brick.GetComponent<MeshRenderer>().bounds.size;
        //Move the animation up by the y size of brick
        playerAnim.transform.position+=new Vector3(0,size.y,0);       
        Transform go=Instantiate(brick, playerAnim.transform.position, brick.transform.rotation);
        // attatch brick to player
        go.SetParent(player.transform);     
        brick.gameObject.SetActive(false);
        noBrick++;
        score=noBrick;
    }

    private void RemoveBrick(GameObject player,GameObject brickBlock){
        Transform brick=player.transform.GetChild(player.transform.childCount-1);       
        GameObject playerAnim= player.transform.GetChild(0).gameObject;
        Destroy(brick.gameObject);
        Vector3 size=brick.GetComponent<MeshRenderer>().bounds.size;
        playerAnim.transform.position-=new Vector3(0,size.y,0);       
        brickBlock.transform.GetChild(0).gameObject.SetActive(true);
        noBrick--;
    }

}
