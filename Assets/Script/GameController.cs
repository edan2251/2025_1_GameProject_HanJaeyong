using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float Timer = 1.0f;
    public GameObject EnemyObject;

    void Update()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            Timer = 1;

            GameObject Temp = Instantiate(EnemyObject);
            Temp.transform.position = new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0);

        }




        if (Input.GetMouseButtonDown(0))                                        //마우스 버튼 눌렀을때
        {   
            RaycastHit hit;                                                     // 물리 Hit 선언
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);        //카메라에서 Ray를 쏴서 3D공간상의 물체를 확인

            if (Physics.Raycast(ray, out hit))                                  //Ray를 쐇을때 Hit 되는 물체가 있으면
            {
                if (hit.collider != null)                                       //물체가 존재하면
                {
                    //Debug.Log($"hit : {hit.collider.name}");                    //해당 물체의 이름을 출력
                    hit.collider.gameObject.GetComponent<Enemy>().CharacterHit(30);     //Enemy 스크립트에서 클릭했을때 30데미지 들어감
                }
            }
        }//.



    }//Update Fin.




}//GameController Fin.
