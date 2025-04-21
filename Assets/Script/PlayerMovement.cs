using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{


    [Header("기본이동설정")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 7.0f;
    public float turnSpeed = 10f;

    [Header("점프개선설정")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;

    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f;            //지면 관성 시간
    public float coyoteTimeCounter;             //관성 타이머
    public bool realGrounded = true;            //실제 지면 상태

    [Header("글라이더 설정")]
    public GameObject gliderObject;
    public float gliderFallSpeed = 1.0f;
    public float gliderMoveSpeed = 7.0f;
    public float gliderMaxTime = 5.0f;
    public float gliderTimeLeft;
    public bool isGliding =false;

    public bool isGrounded = true;

    public int coinCount = 0;
    public int totalCoins = 5;

    public Rigidbody rb;

    void Start()
    {
        //글라이더 오브젝트 초기화
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        gliderTimeLeft = gliderMaxTime;

        coyoteTimeCounter = 0;
    }//StartFin.

    
    void Update()
    {
        //지면 감지 활성화
        UpdateGroundedState();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //이동방향
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        if (movement.magnitude > 0.1f)
        {
            Quaternion largeRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, largeRotation, turnSpeed * Time.deltaTime);
        }

        if(isGrounded)
        {
            if(isGliding)
            {
                DisableGlider();
            }

            gliderTimeLeft = gliderMaxTime;
        }    

        //G키로 글라이더 제어(누르는 동안 활성화
        if(Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0)
        {
            if(!isGliding)
            {
                //글라이더 활성화 함수
                EnableGlider();
            }

            gliderTimeLeft -= Time.deltaTime;

            if(gliderTimeLeft <= 0)
            {
                //글라이더 비활성화 함수 (아래 정의)
                DisableGlider();
            }
        }
        else if (isGliding)
        {
            //G키를 떼면 글라이더 비활성화
            DisableGlider();
        }
        
        if(isGliding)
        {
            ApplyGliderMovement(moveHorizontal, moveVertical);
        }
        else
        {
            //속도값으로 직접 이동
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

            //착지 점프 높이 구현
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }

            

        //Jumpstart
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            realGrounded = false;
            coyoteTimeCounter = 0;
        }

    }//UpdateFin.


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            realGrounded = true;
        }
    }//.

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            realGrounded = true;
        }
    }//.

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            realGrounded = false;
        }
    }//.


    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
            Debug.Log($"코인 수집 : {coinCount}/{totalCoins}");
        }

        if(other.CompareTag("Door") && coinCount >= totalCoins)
        {
            Debug.Log("게임 클리어");
            //끝끝끝끝끝
        }
    }//.

    //지면 상태 업데이트 함수
    void UpdateGroundedState()
    {
        if (realGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            //실제로는 지면에 없지만 코요테 타임 내에 있으면 여전히 지면으로 판단
            if(coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }

    void EnableGlider() //글라이더 활성화 함수
    {
        isGliding = true;

        if(gliderObject != null )
        {
            gliderObject.SetActive(true);
        }

        rb.velocity = new Vector3(rb.velocity.x, -gliderFallSpeed, rb.velocity.z);
    }

    void DisableGlider() //글라이더 비활성화 함수
    {
        isGliding = false;

        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    void ApplyGliderMovement(float horizontal, float vertical)
    {
        //글라이더 효과 : 천천히 떨어지고 수평방향으로 더 빠르게 이동
        Vector3 gliderVelocity = new Vector3(horizontal * gliderMoveSpeed, -gliderFallSpeed, vertical * gliderMoveSpeed);

        rb.velocity = gliderVelocity;
    }

}//
