using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour  //과일 오브젝트에 부착되는 스크립트
{
    //과일 타입 (0 : 사과, 1 : 블루베리, 2 : 코코넛, 3 : 레몬, 4 : 멜론, 5 : 오렌지, 6 : 수박) int 로 선언
    public int fruitType;

    //과일이 이미 합쳐졌는지 확인하는 플래그
    public bool hasMerged = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //이미 합쳐진 과일은 무시
        if (hasMerged)
            return;

        //다른 과일과 충돌했는지 확인
        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

        //충돌한 것이 과일이고 타입이 같다면
        if(otherFruit != null && !otherFruit.hasMerged && otherFruit.fruitType == fruitType)
        {
            //합쳤다고 표시
            hasMerged = true;
            otherFruit.hasMerged = true;

            //두 과일의 중간 위치 계산
            Vector3 mergePosition = (transform.position + otherFruit.transform.position) / 2f;

            //다음 단계 과일로 업그레이드 (추후 구현)
            FruitGame gameManager = FindObjectOfType<FruitGame>();
            if(gameManager != null)
            {
                gameManager.MergeFruits(fruitType, mergePosition);
            }

            //기존 과일 제거
            Destroy(otherFruit.gameObject);
            Destroy(gameObject);
        }
    }
}
