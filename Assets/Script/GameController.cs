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




        if (Input.GetMouseButtonDown(0))                                        //���콺 ��ư ��������
        {   
            RaycastHit hit;                                                     // ���� Hit ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);        //ī�޶󿡼� Ray�� ���� 3D�������� ��ü�� Ȯ��

            if (Physics.Raycast(ray, out hit))                                  //Ray�� �i���� Hit �Ǵ� ��ü�� ������
            {
                if (hit.collider != null)                                       //��ü�� �����ϸ�
                {
                    //Debug.Log($"hit : {hit.collider.name}");                    //�ش� ��ü�� �̸��� ���
                    hit.collider.gameObject.GetComponent<Enemy>().CharacterHit(30);     //Enemy ��ũ��Ʈ���� Ŭ�������� 30������ ��
                }
            }
        }//.



    }//Update Fin.




}//GameController Fin.
