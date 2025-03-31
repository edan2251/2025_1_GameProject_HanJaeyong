using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    
    void Start()
    {
        
    }//StartFIn.

    void Update()
    {
        
    }//UpdateFin.




    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            Debug.Log("땅과 충돌");
        }
    }//.

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("큐브 범위 안에 들어옴");
    }

    


}//BallFin.
