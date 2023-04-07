using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGPlayer : CharacterMovement, IBattle
{
    Transform myTarget = null;

    public bool IsLive { get => !Mathf.Approximately(curHp, 0.0f); }

    void Start()
    {        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            myAnim.SetTrigger("Attack");
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            myAnim.SetTrigger("Skill");
        }
    }
    
    public void OnMove(Vector3 pos)
    {
        if(IsLive) MoveToPos(pos);
    }

    public void OnDamage(float dmg)
    {
        curHp -= dmg;
        if(Mathf.Approximately(curHp, 0.0f))
        {
            // 죽었을 때 Player가 가지고 있는 모든 Collider를 list 화 -> 끄기
            Collider[] list = transform.GetComponentsInChildren<Collider>();
            foreach(Collider col in list)
            {
                col.enabled = false;
            }
            DeathAlarm?.Invoke();
            myAnim.SetTrigger("Dead");
        }
        else
        {
            myAnim.SetTrigger("Damage");
        }
    }

    public void OnAttack()
    {
        myTarget.GetComponent<IBattle>()?.OnDamage(AttackPoint);
    }

    
    public void BeginBattle(Transform target)
    {
        if (!IsLive) return;
        if(myTarget != null)
        {
            // 옵저버 패턴
            // DeathAlarm 은 델리게이트 함수를 활용해 구현
            // 직접 체크하는 것이 아니라 DeathAlarm이 확인하는 방법
            myTarget.GetComponent<CharacterProperty>().DeathAlarm -= TargetDead; // 원래 타겟을 제거
        }
        myTarget = target;
        FollowTarget(myTarget);
        myTarget.GetComponent<CharacterProperty>().DeathAlarm += TargetDead; // 함수 누적 - Delegate 함수가 여러 개의 함수 동시에 참조 가능
    }

    void TargetDead()
    {
        myTarget = null;
        StopAllCoroutines();
    }
}
