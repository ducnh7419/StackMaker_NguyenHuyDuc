using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    private Vector3 buttonDownPoint=Vector3.zero;
    private Vector3 buttonUpPoint=Vector3.zero;
    [SerializeField]private float speed;
    private Stack<GameObject> playerBricks=new Stack<GameObject>(); 

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
        direct = Direct.None;
    }
    

    // Update is called once per frame
    private void Update()
    {
        PlayerControl();      
    }

    private void PlayerControl(){
        if(Input.GetMouseButtonDown(0)){
            buttonDownPoint=Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0)){
            buttonUpPoint=Input.mousePosition;
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
        switch(direct){
                case Direct.Forward:
                    Debug.Log("Forward");
                    AddOrRemoveBrick();
                    rb.velocity=speed * Time.deltaTime * Vector3.forward;
                    rb.rotation=Quaternion.Euler(0,0,0);
                    break;
                case Direct.Back:
                    Debug.Log("Back");
                    AddOrRemoveBrick();
                    rb.velocity=speed * Time.deltaTime * Vector3.back;
                    rb.rotation=Quaternion.Euler(Vector3.down*180);
                    break;
                case Direct.Left:
                    Debug.Log("Left");
                    AddOrRemoveBrick();
                    rb.velocity=speed * Time.deltaTime * Vector3.left;
                    rb.rotation=Quaternion.Euler(Vector3.down*90);
                    break;
                case Direct.Right:
                    Debug.Log("Right");
                    AddOrRemoveBrick();
                    rb.velocity=speed * Time.deltaTime * Vector3.right;
                    rb.rotation=Quaternion.Euler(Vector3.up*90);
                    break;
                case Direct.None:
                    break;
        }      
    }

    private void AddOrRemoveBrick(){
        GameObject block=new GameObject();
        if(CheckBrick(ref block)){
            AddBrick(rb.gameObject,block);
        }else{
            RemoveBrick(rb.gameObject,block);
            
        }
    }

    private bool CheckBrick(ref GameObject block){
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            block=hit.collider.gameObject;
            Transform child=hit.collider.gameObject.transform.GetChild(0);
            if(child.CompareTag("brick")){
                if(child.gameObject.activeSelf){
                    return true;
                }
            }
        }
        return false;
    }

    private void AddBrick(GameObject player,GameObject brickBlock){
        // Get Brick GameObject of brick block
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
        playerBricks.Push(brick.gameObject);
    }

    private void RemoveBrick(GameObject player,GameObject brickBlock){
        Transform brick=brickBlock.transform.GetChild(0);
        GameObject playerAnim= player.transform.GetChild(0).gameObject;
        if(playerBricks.Count>0){
            GameObject destroyBrick=playerBricks.Pop();
            Destroy(destroyBrick);
            Vector3 size=brick.GetComponent<MeshRenderer>().bounds.size;
            playerAnim.transform.position-=new Vector3(0,size.y,0);
        
            brick.gameObject.SetActive(true);
        }
    }
}
