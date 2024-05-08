using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrigger : MonoBehaviour
{

   private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("wall"))
            Debug.Log("A");
        else{
            Debug.Log("B");
        }
   }
}
