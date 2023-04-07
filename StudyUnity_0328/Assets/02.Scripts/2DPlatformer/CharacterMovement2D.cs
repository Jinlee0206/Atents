using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterMovement2D : CharacterProperty2D
{
    float defaultForward = 1.0f;
    Coroutine coJump = null;
    bool isDown = false;                // 점프 최대치에 도달해서 내려오고 있는 경우
    float dropDist = -1.0f;             // 바닥체크를 시작하기 시작하는 지점 (떨어질 거리)
    float dropHeight = 0.0f;            // 떨어지기 시작하는 지점의 높이

    protected void MoveByDir(Vector2 dir, UnityAction done = null)
    {
        StartCoroutine(MovingByDirection(dir, done));
    }

    protected void Jump(float totalTime, float maxHeight)
    {
        if (coJump != null) StopCoroutine(coJump);
        coJump = StartCoroutine(Jumping(totalTime, maxHeight));
    }

    protected void Drop()
    {
        Jump(0.5f, 0.5f);                       // 떨어질 때 살짝 점프하는 효과로 처리
        dropHeight = transform.position.y;      // 현재 높이를 dropHeight로 저장
        dropDist = 1.5f;
    }

    protected void Attack(Transform target)
    {
        StartCoroutine(Attacking(target));
    }

    // 한 층을 움직이게 하는 코루틴 함수
    IEnumerator MovingByDirection(Vector2 dir, UnityAction done)
    {
        bool cliff = false;
        myAnim.SetBool("isMoving", true);
        while (!cliff)
        {
            //transform.Translate(dir * MoveSpeed * Time.deltaTime, Space.World); // Translate 이용한 단순 이동

            if (!myAnim.GetBool("isAir")) // 공중에 떠있는 상태가 아니라면
            {
                Vector3 pos = transform.position + (Vector3)dir * MoveSpeed * Time.deltaTime;
                cliff = checkCliff(pos + Vector3.up * 0.5f);
                if (cliff)
                {
                    break;
                }
                transform.position = pos;
            }
            
            yield return null;
        }
        myAnim.SetBool("isMoving", false);
        done?.Invoke();
    }

    // 삼각함수와 코루틴를 이용한 점프구현
    IEnumerator Jumping(float totalTime, float maxHeight)    // 체공 시간, 최고 높이
    {
        isDown = false;
        float t = 0.0f; // 처음시작 시간
        float orgHeight = transform.position.y; // 처음 높이 값 저장

        myAnim.SetTrigger("Jump");

        while (t <= totalTime)
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


    //// 아래 방향키나 S를 눌렀을 때 강하
    //IEnumerator Droping()
    //{
    //    Jump(1.0f, 1.0f);
    //    dropDist = 1.5f;
    //    yield return null;
    //}

    public LayerMask crashMask;                 // 바닥이 되는 마스크
    // 레이를 쏴서 바닥에 도착했는지 확인하는 함수
    protected void AirCheck()
    {
        Vector2 orgPos = transform.position + Vector3.up * 0.5f;
        Vector2 dir = Vector2.down;

        RaycastHit2D hit = Physics2D.Raycast(orgPos, dir, 1.0f, crashMask);
        Debug.DrawLine(orgPos, orgPos + dir * 1.0f, Color.red);
        if (hit.collider != null && dropDist < 0.0f)
        {
            if (isDown)
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

            if (dropDist > 0.0f)
            {
                float dist = dropHeight - transform.position.y;
                if (dist > dropDist)
                {
                    dropDist = -1.0f;
                }

                //dropDist -= delta;        // 기존
            }
        }
    }

    bool checkCliff(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 1.0f, crashMask);
        if (hit.collider == null)
        {
            return true;
        }
        return false;
    }

    protected void Turn()
    {
        myRenderer.flipX = !myRenderer.flipX;
    }

    protected Vector3 Forward(){ return myRenderer.flipX? transform.right * defaultForward : -transform.right; }

    IEnumerator Attacking(Transform target)
    {
        float playTime = 0.0f;
        while(target!= null)
        {
            playTime += Time.deltaTime;
            Vector3 dir = target.position - transform.position; // 자신에게서 대상에게로 향하는 벡터
            if(dir.magnitude <= AttackRange)
            {
                if(playTime >= AttackDelay)
                {
                    myAnim.SetTrigger("Attack");
                    playTime = 0.0f;
                }
            }
            // 공격 범위 밖으로 대상이 벗어났을 때 -> 추격해야한다
            else
            {
                dir.y = 0.0f;                                   // x축 평행이동만 하게끔 y값을 0으로 만들어 줌
                float dist = dir.magnitude - AttackRange;
                if (dist < 0.0f) dist = 0.0f;
                dir.Normalize();
                float delta = MoveSpeed * Time.deltaTime;
                if(delta > dist) delta = dist;
                transform.Translate(dir * delta);
            }
            yield return null;
        }
    }
}
