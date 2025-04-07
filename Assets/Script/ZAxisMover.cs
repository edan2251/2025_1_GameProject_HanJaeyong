using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//5초동안 5의 스피드로 앞을 향해 이동하고 사라지는 컴포넌트 클래스(오브젝트에서의 앞 = Z축)
public class ZAxisMover : MonoBehaviour
{
    public float speed = 5.0f;
    public float timer = 5.0f;


    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Destroy(gameObject);
        }
    }//.Update
}
