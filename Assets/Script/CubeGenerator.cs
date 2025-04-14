using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{

    public GameObject cubePrefab;
    public int totalCubes = 10;
    public float cubeSpacing = 1.0f;

    
    void Start()
    {
        GenCube();
    }

    public void GenCube()                                                                       
    {               
        Vector3 myPosition = transform.position;
        GameObject firstCube = Instantiate(cubePrefab, myPosition, Quaternion.identity);

        for (int i = 1; i < totalCubes; i++)
        {
            Vector3 position = new Vector3(myPosition.x, myPosition.y, myPosition.z + (i * cubeSpacing));
            Instantiate(cubePrefab, position, Quaternion.identity);
        }

    }
}
