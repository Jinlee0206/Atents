using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster2D : CharacterMovement2D, IBattle
{
    public enum State
    {
        Create, Normal, Battle
    }

    public State myState = State.Create;
    public Transform myTarget = null;



    public bool IsLive
    {
        get
        {
            return true;
        }
    }

    void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case State.Create:
                break;
            case State.Normal:
                MoveByDir(-transform.right, () => TurnMove()); // 절벽에 도달하면 Turn(flipX)후 전방을 바꿔준 후 이동
                break;
            case State.Battle:
                StopAllCoroutines();
                Attack(myTarget);
                break;
        }
    }

    void StateProcess(State s)
    {
        myState = s;
        switch (myState)
        {
            case State.Create:
                break;
            case State.Normal:
                break;
            case State.Battle:
                break;
        }        
    }

    private void Start()
    {
        ChangeState(State.Normal);
    }

    // 방향 전환하는 함수 재귀적으로 구현
    void TurnMove()
    {
        Turn();
        MoveByDir(Forward(), TurnMove);
    }

    // 
    public void FindEnemy(Transform target)
    {
        myTarget = target;
        ChangeState(State.Battle);
    }

    public void OnAttack()
    {
        myTarget.GetComponent<IBattle>()?.OnDamage(AttackPoint);    // myTarget이 IBattle 컴포넌트를 상속하고 있으면 OnDamage 함수 호출
    }

    public void OnDamage(float dmg)
    {
        myAnim.SetTrigger("Damage");
    }

}
