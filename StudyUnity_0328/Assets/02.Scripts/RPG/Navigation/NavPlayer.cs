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
        // �ִϸ��̼� ���� Ʈ���� ����� �� 
        myAnim.SetFloat("Speed", myNav.velocity.magnitude / myNav.speed);
        //myAnim.SetFloat("Rotaion", myNav.myNav.angularSpeed);
    }

    public void OnWarp(Vector3 pos)
    {
        //transform.position = pos; // NavMeshAgent�� �� ã�� �� �ִ� ���� �� ���ִ�
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
        myNav.SetDestination(pos); // �ڷ�ƾ ���·� �������ִ� �޼ҵ�� �����ϸ� �ȴ�
                                   // ���������� ������ �������� remainingDistance�� ���´�
        myAnim.SetBool("isMoving", true);
        // myNav.pathPending => ����� �������� bool�� ����
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
            // ���� �����޽ø�ũ�� �ִ°�?
            if (myNav.isOnOffMeshLink)
            {
                myAnim.SetBool("isJumping", true);
                // NavMeshAgent �ߴ��ؼ� Jump �ӵ� �� ��ġ�� ���� ����
                myNav.isStopped = true;
                Vector3 endPos = myNav.currentOffMeshLinkData.endPos; // ���� offmeshlinkdata�� ������
                
                // �Ÿ�, ������ ���ؼ� ���� ����
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
