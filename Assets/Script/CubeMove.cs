using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    
    void Update()
    {
        transform.Translate(0, 0, -moveSpeed * Time.deltaTime);

        if(transform.position.z < -20)
        {
            Destroy(gameObject);
        }
    }
}
