using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterMovement : CharacterProperty
{
    Coroutine coMove = null;            // 이전 MovingToPos 코루틴 함수 중지를 위해 Coroutine 인스턴스 생성

    protected void MoveToPos(Vector3 pos, UnityAction done = null)
    {
        if(coMove != null)
        {
            StopCoroutine(coMove);
            coMove = null;
        }
        coMove = StartCoroutine(MovingToPos(pos, done));
    }

    protected void FollowTarget(Transform target)
    {
        StopAllCoroutines();
        StartCoroutine(FollowingTarget(target));
    }

    IEnumerator MovingToPos(Vector3 pos, UnityAction done)
    {
        Vector3 dir = pos - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();

        StartCoroutine(Rotating(dir));

        myAnim.SetBool("isMoving", true);

        while(dist > 0.0f)
        {
            if (!myAnim.GetBool("isAttacking"))
            {
                float delta = MoveSpeed * Time.deltaTime;
                if (dist - delta < 0.0f)
                {
                    delta = dist;
                }
                dist -= delta;
                transform.Translate(dir * delta, Space.World);
            }
            yield return null;
        }

        myAnim.SetBool("isMoving", false);
        done?.Invoke();                         // done 이 null이 아니라면 Invoke();
    }

    //IEnumerator MovingToPos(Vector3[] pathList)
    //{
    //    int i = 1;
    //    while(i < pathList.Length)
    //    {
    //        bool done = false;
    //        MoveToPos(pathList[i], () => done = true);
    //        while (!done) yield return null;
    //        i++;
    //    }
    //}

    IEnumerator Rotating(Vector3 dir)
    {
        float angle = Vector3.Angle(transform.forward, dir);
        float rotDir = 1.0f;
        if(Vector3.Dot(transform.right,dir) < 0.0f)
        {
            rotDir = -1.0f;
        }
        while(angle > 0.0f)
        {
            float delta = RotSpeed * Time.deltaTime;
            if(angle - delta < 0.0f)
            {
                delta = angle;
            }
            angle -= delta;
            transform.Rotate(Vector3.up * rotDir * delta);
            yield return null;
        }
    }

    IEnumerator FollowingTarget(Transform target)
    {
        while(target != null)
        {
            // 매 프레임 확인하는 방법
            //if(target.GetComponent<IBattle>() != null)
            //{
            //    if (!target.GetComponent<IBattle>().IsLive) yield break;
            //}
            if(!myAnim.GetBool("isAttacking")) playTime += Time.deltaTime;
            if (!myAnim.GetBool("isAttacking"))
            {
                myAnim.SetBool("isMoving", false);
                Vector3 dir = target.position - transform.position;
                float dist = dir.magnitude - AttackRange;                
                dir.Normalize();
                float delta = 0.0f;

                if (dist > 0.0f)
                {
                    delta = MoveSpeed * Time.deltaTime;
                    if (dist <= delta)
                    {
                        delta = dist;                        
                    }
                    myAnim.SetBool("isMoving", true);
                    transform.Translate(dir * delta, Space.World);
                }
                else
                {
                    if (!myAnim.GetBool("isAttacking"))
                    {
                        if (playTime >= AttackDelay)
                        {
                            playTime = 0.0f;
                            myAnim.SetTrigger("Attack");
                        }
                    }
                }

                //Rotation
                float angle = Vector3.Angle(transform.forward, dir);
                float rotDir = 1.0f;
                if (Vector3.Dot(transform.right, dir) < 0.0f)
                {
                    rotDir = -1.0f;
                }
                delta = RotSpeed * Time.deltaTime;
                if (angle < delta)
                {
                    delta = angle;
                }
                transform.Rotate(transform.up * rotDir * delta, Space.World);
            }

            yield return null;
        }
    }

  
}
