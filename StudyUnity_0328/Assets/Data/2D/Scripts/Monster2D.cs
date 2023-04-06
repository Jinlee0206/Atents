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
                MoveByDir(-transform.right, () => TurnMove()); // ������ �����ϸ� Turn(flipX)�� ������ �ٲ��� �� �̵�
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

    // ���� ��ȯ�ϴ� �Լ� ��������� ����
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
        myTarget.GetComponent<IBattle>()?.OnDamage(AttackPoint);    // myTarget�� IBattle ������Ʈ�� ����ϰ� ������ OnDamage �Լ� ȣ��
    }

    public void OnDamage(float dmg)
    {
        myAnim.SetTrigger("Damage");
    }

}
