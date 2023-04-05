using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : CharacterProperty2D
{

    void Start()
    {
    }

    private void FixedUpdate()
    {
        AirCheck();
    }

    void Update()
    {
        Vector2 dir = Vector2.zero;
        dir.x = Input.GetAxisRaw("Horizontal");

        if (!Mathf.Approximately(dir.x, 0.0f))
        {
            myAnim.SetBool("isMoving", true);
            if (dir.x < 0.0f)
            {
                myRenderer.flipX = true;
            }
            else
            {
                myRenderer.flipX = false;
            }
        }
        else
        {
            myAnim.SetBool("isMoving", false);
        }

        //transform.position = new Vector2(x, y) * Time.deltaTime * MoveSpeed;
        transform.Translate(new Vector2(dir.x * Time.deltaTime * MoveSpeed, 0));

        if (Input.GetMouseButtonDown(0))
        {
            if (!myAnim.GetBool("isAttacking"))
            {
                myAnim.SetTrigger("Attack");
            }
        }

        if(Input.GetKeyDown(KeyCode.Space) && !myAnim.GetBool("isAir"))
        {
            //myRigid2D.AddForce(new Vector2(0, 400.0f));     // Rigidbody2D 물리를 이용한 점프 구현
            if (coJump != null) StopCoroutine(coJump);
            coJump = StartCoroutine(Jumping(1.0f, 3.0f));
        }

        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(Droping());
        }
    }

    Coroutine coJump = null;
    bool isDown = false;            // 점프 최대치에 도달해서 내려오고 있는 경우
    // 삼각함수를 이용한 점프구현
    IEnumerator Jumping(float totalTime, float maxHeight)    // 체공 시간, 최고 높이
    {
        isDown = false;
        float t = 0.0f; // 처음시작 시간
        float orgHeight = transform.position.y; // 처음 높이 값 저장

        myAnim.SetTrigger("Jump");

        while(t <= totalTime)
        {
            if (t >= totalTime * 0.5f) isDown = true;
            t += Time.deltaTime;
            float h = Mathf.Sin(Mathf.PI * (t / totalTime)) * maxHeight;    // t : totalTime = h : PI 비례식 이용

            // 점프를 하는 와중에 Ray를 쏴서 프레임 단위로 떨어질 때

            Vector3 pos = new Vector3(transform.position.x, orgHeight + h, transform.position.z);

            if (isDown)
            {
                RaycastHit2D hit = 
                    Physics2D.Raycast(transform.position, Vector2.down, Vector3.Distance(transform.position, pos), crashMask);
                if (hit.collider != null)
                {
                    transform.position = hit.point;
                    yield break;
                }
            }

            transform.position = pos;   // 높이 값만 변화시켜줘서 transform.position에 대입
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, orgHeight, transform.position.z);
    }

    float dropDist = 0.0f;

    // 아래 방향키나 S를 눌렀을 때 강하
    IEnumerator Droping()
    {
        dropDist = 1.0f;
        yield return null;
    }

    public LayerMask crashMask;                 // 바닥이 되는 마스크
    // 레이를 쏴서 바닥에 도착했는지 확인하는 함수
    void AirCheck()
    {
        Vector2 orgPos = transform.position + Vector3.up * 0.05f; 
        Vector2 dir = Vector2.down;

        RaycastHit2D hit = Physics2D.Raycast(orgPos, dir, 0.2f, crashMask);
        if (hit.collider != null && Mathf.Approximately(dropDist, 0.0f))
        {
            if(isDown)
            {
                if (coJump != null) StopCoroutine(coJump);
                transform.position = hit.point;
            }
            myAnim.SetBool("isAir", false); // 레이를 쐈을 때 검출된 것이 있다
        }
        else
        {
            myAnim.SetBool("isAir", true);                              // 레이를 쐈을 때 검출된 것이 없다
            float delta = 9.8f * Time.deltaTime;
            transform.position += Vector3.down * delta; // 강제로 중력 작동 (Rigidbody 없어서 플레이어 물리가 없다)

            if(dropDist > 0.0f)
            {
                dropDist -= delta;
            }
        }
    }


}
