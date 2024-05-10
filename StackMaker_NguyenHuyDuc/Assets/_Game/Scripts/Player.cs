using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    private Vector3 buttonDownPoint=Vector3.zero;
    private Vector3 buttonUpPoint=Vector3.zero;
    [SerializeField]private float speed;
    private bool isMoving;
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
        isMoving=false;
        direct = Direct.None;
    }
    

    // Update is called once per frame
    private void Update()
    {
        PlayerControl();      
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
                    if(!CheckWall(directionVector)){
                        isMoving=true;
                        AddOrRemoveBrick();
                        rb.velocity=speed * Time.deltaTime * directionVector;
                    }else{
                        isMoving=false;
                        direct=Direct.None;
                        rb.velocity=Vector3.zero;
                    }
                    rb.rotation=Quaternion.Euler(0,0,0);
                    // rb.rotation=Quaternion.Euler(Vector3.up*180);
                    break;
                case Direct.Back:
                    directionVector=Vector3.back;
                    if(!CheckWall(directionVector)){
                        isMoving=true;
                        AddOrRemoveBrick();
                        rb.velocity=speed * Time.deltaTime * directionVector;
                    }else{
                        isMoving=false;
                        direct=Direct.None;
                        rb.velocity=Vector3.zero;
                    }
                    // rb.rotation=Quaternion.Euler(0,0,0);
                    rb.rotation=Quaternion.Euler(Vector3.down*180);
                    break;
                case Direct.Left:
                    directionVector=Vector3.left;
                    if(!CheckWall(directionVector)){
                        isMoving=true;
                        AddOrRemoveBrick();
                        rb.velocity=speed * Time.deltaTime * directionVector;
                    }else{
                        isMoving=false;
                        direct=Direct.None;
                        rb.velocity=Vector3.zero;
                    }           
                    rb.rotation=Quaternion.Euler(Vector3.down*90);
                    break;
                case Direct.Right:
                    directionVector=Vector3.right;
                    if(!CheckWall(directionVector)){
                        isMoving=true;
                        AddOrRemoveBrick();
                        rb.velocity=speed * Time.deltaTime * directionVector;
                    }else{
                        isMoving=false;
                        rb.velocity=Vector3.zero;
                    }
                    rb.rotation=Quaternion.Euler(Vector3.up*90);
                    break;
                case Direct.None:
                    isMoving=false;
                    rb.velocity=Vector3.zero;
                    break;
        }
        ReachingChest(ref direct);
             
    }

    private void AddOrRemoveBrick(){
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f))
        {
            
            GameObject block=hit.collider.gameObject;
            if(block.CompareTag("brick")){
                AddBrick(rb.gameObject,block);}
            if(block.CompareTag("bridge")){
                RemoveBrick(rb.gameObject,block);                
            }
            if(block.CompareTag("Finish Bridge")){          
                block.transform.parent.GetChild(1).GetChild(0).GetComponent<Renderer>().material.color=Color.blue;
                block.transform.parent.GetChild(2).GetChild(0).GetComponent<Renderer>().material.color=Color.green;
                RemoveBrick(rb.gameObject,block);
            }
            block.GetComponent<Collider>().enabled=false;
        }
    }

    private void ReachingChest( ref Direct direct){
        if (Physics.Raycast(transform.position, Vector3.right , out RaycastHit hit, 1f)){
            if(hit.collider.gameObject.CompareTag("Chest")){
                Debug.Log("Hit");
                GameObject closeChest=hit.collider.gameObject;
                GameObject openChess=closeChest.transform.parent.GetChild(closeChest.transform.parent.childCount-1).gameObject;
                openChess.SetActive(true);
                closeChest.SetActive(false);
                direct=Direct.None;
                GameManager.Ins.ChangeState(GameManager.State.EndGame);
            }
            
        }
    }

    private bool CheckWall(Vector3 direction){       
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, .5f))
        {
            GameObject block=hit.collider.gameObject;
            if(block.CompareTag("wall")){
                return true;
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
        // go.transform.GetComponent<Collider>().enabled=true;
        // attatch brick to player
        go.SetParent(player.transform);     
        brick.gameObject.SetActive(false);
    }

    private void RemoveBrick(GameObject player,GameObject brickBlock){
        Transform brick=player.transform.GetChild(player.transform.childCount-1);       
        GameObject playerAnim= player.transform.GetChild(0).gameObject;
        Destroy(brick.gameObject);
        Vector3 size=brick.GetComponent<MeshRenderer>().bounds.size;
        playerAnim.transform.position-=new Vector3(0,size.y,0);       
        brickBlock.transform.GetChild(0).gameObject.SetActive(true);       
    }

    private  void OnDrawGizmos() {
        Vector3 direction=Vector3.forward;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(direction) * .5f);
    }
}
