using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : CharacterMovement, IPerception, IBattle
{
    public static int TotalCount = 0; // ���� ���� : ���� �� �� ���� �����Ǵ� ������ Ÿ�� ���  

    public enum State
    {
        Create, Normal, Battle, Death
    }
    public State myState = State.Create;


    // ������Ʈ �ӽ��� �������
    // ������Ʈ

    // �� ������Ʈ�� �����ϴ� Dictionary
    
    Vector3 orgPos;

    public Transform myTarget = null;
    
    public bool IsLive { get => myState != State.Death; }
    
    void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;
        switch(myState)
        {
            case State.Normal:
                myAnim.SetBool("isMoving", false);
                StopAllCoroutines();
                StartCoroutine(Roaming(Random.Range(1.0f, 3.0f)));
                break;
            case State.Battle:
                StopAllCoroutines();
                FollowTarget(myTarget);
                break;
            case State.Death:
                Collider[] list = transform.GetComponentsInChildren<Collider>();
                foreach (Collider col in list) col.enabled = false;
                DeathAlarm?.Invoke(); // ���� �� ��� ������Ʈ���� �˷��ֱ�
                StopAllCoroutines();
                myAnim.SetTrigger("Dead");
                
                break;
            default:
                Debug.Log("ó�� ���� �ʴ� ���� �Դϴ�.");
                break;
        }
    }
    void StateProcess()
    {
        switch (myState)
        {
            case State.Normal:
                break;
            case State.Battle:
                break;
            case State.Death:
                break;
            default:
                Debug.Log("ó�� ���� �ʴ� ���� �Դϴ�.");
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        TotalCount++;
        orgPos = transform.position;
        ChangeState(State.Normal);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    IEnumerator Roaming(float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 pos = orgPos;
        pos.x += Random.Range(-5.0f, 5.0f);
        pos.z += Random.Range(-5.0f, 5.0f);
        MoveToPos(pos, ()=> StartCoroutine(Roaming(Random.Range(1.0f,3.0f))));  // ������ MoveToPos �Լ� ����ϴµ�, ��������Ʈ (UnityAction) ����ؼ�
                                                                                // MoveToPos�� ��ȣ�� 
    }

    public void Find(Transform target)
    {
        myTarget = target;
        myTarget.GetComponent<CharacterProperty>().DeathAlarm += () => { if (IsLive) ChangeState(State.Normal); }; // �÷��̾ �����ϴ� ����� �븻���·� �����
        ChangeState(State.Battle);
    }

    public void LostTarget()
    {
        myTarget = null;
        ChangeState(State.Normal);
    }

    public void OnAttack()
    {
        myTarget.GetComponent<IBattle>()?.OnDamage(AttackPoint);
    }

    public void OnDamage(float dmg)
    {
        Debug.Log($"{curHp}");
        curHp -= dmg;
        if(Mathf.Approximately(curHp , 0.0f))
        {
            ChangeState(State.Death);
        }
        else
        {
            myAnim.SetTrigger("Damage");
        }
    }

    public void OnDisappeaing()
    {
        StartCoroutine(Disappearing());
    }

    IEnumerator Disappearing()
    {
        //while (true)
        //{
        //    Debug.Log($"{delay}");
        //    if (delay > 0.0f)
        //    {
        //        float delta = Time.deltaTime;
        //        delay -= delta;
        //    }
        //    else
        //    {
        //        transform.Translate(Vector3.down * 0.5f * Time.deltaTime);
        //        Destroy(this.gameObject, 1.5f); // �����丵�� �Ұ��� -> ��ȿ����
        //    }
        //    yield return null;
        //}


        // �׾��� �� �����̸� ����
        yield return new WaitForSeconds(3.0f);

        // ���� ���� (y) ���Ϸ� �������ٰ� �ش� ������Ʈ �ı�
        float dist = 0.0f;
        while(dist < 1.0f)
        {
            dist += Time.deltaTime;
            transform.Translate(Vector3.down * 0.5f * Time.deltaTime);
            yield return null;
        }
        Destroy(this.gameObject);
        TotalCount--;
    }
}
