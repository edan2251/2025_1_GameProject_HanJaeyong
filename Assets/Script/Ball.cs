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
            Debug.Log("���� �浹");
        }
    }//.

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ť�� ���� �ȿ� ����");
    }

    


}//BallFin.
