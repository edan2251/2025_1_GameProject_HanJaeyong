using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleBallController : MonoBehaviour
{
    [Header("기본설정")]
    public float        power = 10f;                    //타격 힘
    public Sprite       arrowSprite;                    //화살표 이미지

    private Rigidbody   rb;                             //공의 물리
    private GameObject  arrow;                          //화살표 오브젝트
    private bool        isDragging = false;             //드래그 중인지
    private Vector3     startPos;                       //드래그 시작 위치

    //턴 관리를 위한 전역 변수(모든 공이 공유)(하나가 true 면 전부 true)
    static bool         isAnyBallPlaying = false;       //어떤 공이라도 턴 중인지
    static bool         isAnyBallMoving = false;       //어떤 공이라도 움직이는지


    // Start is called before the first frame update
    void Start()
    {
        SetupBall();        //시작할 때 초기화
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        UpdateArrow();
    }

    void SetupBall()                                    //공 설정하기
    {
        rb = GetComponent<Rigidbody>();                 //물리 컴포넌트 가져오기
        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();  //없을경우 붙여준다.
        }

        //물리 설정
        rb.mass = 1;
        rb.drag = 1;
    }

    public bool isMoving()                              //공이 움직이고 있는지 확인
    {
        return rb.velocity.magnitude > 0.2f;            //공이 속도를 가지고 있으면 움직인다고 판단
    }

    void HandleInput()                                  //입력 처리하기
    {
        //턴 매니저가 허용하지 않으면 조작 불가
        if (!SimpleTurnManager.canPlay) return;

        //다른 공이 움직일 때
        if (SimpleTurnManager.anyBallMoving) return;

        //공이 움직이고 있으면 조작 불가
        if (isMoving()) return;
        
        if (Input.GetMouseButtonDown(0))                //마우스 클릭 시작
        {
            StartDrag();
        }

        if(Input.GetMouseButtonUp(0) && isDragging)     //마우스 버튼 떼기
        {
            Shoot();
        }
    }

    void StartDrag()        //드래그 시작 함수
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                startPos = Input.mousePosition;
                CreateArrow();
                Debug.Log("드래그 시작");
            }
        }


    }

    void Shoot()            //공 발사 하기
    {
        Vector3 mouseDelta = Input.mousePosition - startPos;    //마우스 이동 거리로 힘 계산
        float force = mouseDelta.magnitude * 0.01f * power;

        if (force < 5) force = 5;                               //최소 힘 보정

        Vector3 direction = new Vector3(-mouseDelta.x, 0, -mouseDelta.y).normalized;   //방향 계산

        rb.AddForce(direction * force, ForceMode.Impulse);      //공에 힘 적용

        //턴 매니저에게 공을 쳤다고 알림
        SimpleTurnManager.OnBallHit();

        //정리
        isDragging = false;
        Destroy(arrow);
        arrow = null;

        Debug.Log("[발사] 힘 : " + force);

    }

    void CreateArrow()                                                      //화살표 만들기
    {
        if(arrow != null)                                                   //기존 화살표 제거
        {
            Destroy(arrow);
        }

        arrow = new GameObject("Arrow");                                    //새 화살표 만들기
        SpriteRenderer sr = arrow.AddComponent<SpriteRenderer>();

        sr.sprite = arrowSprite;                                            //화살표 이미지 설정
        sr.color = Color.green;
        sr.sortingOrder = 10;

        arrow.transform.position = transform.position + Vector3.up;         //화살표 위치 (공 위에)
        arrow.transform.localScale = Vector3.one;
    }

    void UpdateArrow()                                                      //화살표 업데이트
    {
        if(!isDragging || arrow == null) return;

        Vector3 mouseDelta = Input.mousePosition - startPos;                //마우스 이동 거리 계산
        float distance = mouseDelta.magnitude;

        float size = Mathf.Clamp(distance * 0.01f, 0.5f, 2f);               //화살표 크기 변경(힘에 따라)
        arrow.transform.localScale = Vector3.one * size;

        SpriteRenderer sr = arrow.GetComponent<SpriteRenderer>();           //화살표 색상 변경(힘에 따라 초록 -> 빨강)
        float colorRatio = Mathf.Clamp01(distance * 0.005f);
        sr.color = Color.Lerp(Color.green, Color.red, colorRatio);

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);

        if (distance > 10f) //최소 거리 이상 드래그 했을때
        {
            Vector3 direction = new Vector3(-mouseDelta.x, 0, -mouseDelta.y);
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;        //방향을 잡아 주는 공식
            arrow.transform.rotation = Quaternion.Euler(90, angle, 0);                  //화살표 방향 설정
        }
    }
}
