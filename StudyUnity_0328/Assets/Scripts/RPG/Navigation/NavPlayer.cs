using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavPlayer : CharacterProperty
{
    public NavMeshAgent myNav;

    private void Awake()
    {
        myNav = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
    }

    void Update()
    {
        // 애니메이션 블렌드 트리로 만들어 둔 
        myAnim.SetFloat("Speed", myNav.velocity.magnitude / myNav.speed);
        //myAnim.SetFloat("Rotaion", myNav.myNav.angularSpeed);
    }

    public void OnWarp(Vector3 pos)
    {
        //transform.position = pos; // NavMeshAgent가 길 찾을 수 있는 곳만 갈 수있다
        myNav.Warp(pos);
    }

    public void OnMove(Vector3 pos)
    {
        //myNav.SetDestination(pos);
        //StartCoroutine(Moving(pos));

        if (myAnim.GetBool("isJumping")) return;
        StopAllCoroutines();
        StartCoroutine(JumpableMoving(pos));
    }
    
    IEnumerator Moving(Vector3 pos)
    {
        myNav.SetDestination(pos); // 코루틴 형태로 구성되있는 메소드라 생각하면 된다
                                   // 내부적으로 연산이 끝나야지 remainingDistance가 나온다
        myAnim.SetBool("isMoving", true);
        // myNav.pathPending => 계산이 끝났는지 bool값 리턴
        while(myNav.pathPending || myNav.remainingDistance > myNav.stoppingDistance)
        {
            yield return null;
        }
        myAnim.SetBool("isMoving", false);
    }

    IEnumerator JumpableMoving(Vector3 pos)
    {
        myNav.SetDestination(pos);
        while(myNav.pathPending || myNav.remainingDistance > myNav.stoppingDistance)
        {
            // 현재 오프메시링크에 있는가?
            if (myNav.isOnOffMeshLink)
            {
                myAnim.SetBool("isJumping", true);
                // NavMeshAgent 중단해서 Jump 속도 및 위치값 직접 지정
                myNav.isStopped = true;
                Vector3 endPos = myNav.currentOffMeshLinkData.endPos; // 현재 offmeshlinkdata의 끝지점
                
                // 거리, 방향을 구해서 수동 조작
                Vector3 dir = endPos - transform.position;
                float dist = dir.magnitude;
                dir.Normalize();
                while(dist > 0.0f)
                {
                    float delta = myNav.speed * Time.deltaTime;
                    if (dist < delta) delta = dist;
                    dist -= delta;
                    transform.Translate(delta * dir, Space.World);
                    yield return null;
                }
                myAnim.SetBool("isJumping", false);
                myNav.CompleteOffMeshLink();
                myNav.isStopped = false;
                myNav.velocity = dir * myNav.speed;
                Debug.Log($"{myNav.velocity.magnitude}");
            }
            yield return null;
        }
    }
}
