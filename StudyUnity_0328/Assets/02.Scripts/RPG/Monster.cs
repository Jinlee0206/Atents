using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Monster : CharacterMovement, IPerception, IBattle
{
    public static int TotalCount = 0; // 정적 변수 : 빌드 할 때 부터 생성되는 컴파일 타임 상수  

    public enum State
    {
        Create, Normal, Battle, Death
    }
    public State myState = State.Create;


    // 스테이트 머신을 끌어오고
    // 스테이트

    // 각 스테이트를 저장하는 Dictionary
    
    Vector3 orgPos;

    public Transform myTarget = null;
    public Transform myHeadPoint = null;

    UnityAction deadAction = null; // 몬스터가 죽을 때 처리되는 함수 

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
                DeathAlarm?.Invoke(); // 죽을 때 모든 오브젝트에게 알려주기
                StopAllCoroutines();
                myAnim.SetTrigger("Dead");
                
                break;
            default:
                Debug.Log("처리 되지 않는 상태 입니다.");
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
                Debug.Log("처리 되지 않는 상태 입니다.");
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        TotalCount++;
        orgPos = transform.position;
        ChangeState(State.Normal);

        GameObject obj = Instantiate(Resources.Load("MonsterHPBar"), SceneData.Inst.hpBars) as GameObject;
        HPBarUI hpBarUI = obj.GetComponent<HPBarUI>();

        // 생성된 게임오브젝트 obj(hpBarUI 컴포넌트를 가짐) 가 canvas를 찾는 방법
        //Canvas canvas = FindObjectOfType<Canvas>(); // 씬에 존재하는 모든 오브젝트를 검사하면서 찾음(비용이 비쌈), 캔버스가 하나인 경우만 사용
        //GameObject objCanvas = GameObject.Find("Canvas"); // Find 계열의 함수(비용이 비쌈), 스트링 사용 오타 유발이 야기됨

        // GameManager를 생성해 놓고 거기서 Canvas 오브젝트를 static으로 선언 및 바인딩

        hpBarUI.myRoot = myHeadPoint;
        updateHP.AddListener(hpBarUI.updateHP); // UnityEvent 누적함수
        deadAction += () => Destroy(hpBarUI.gameObject);

        MiniMapIcon icon =
            (Instantiate(Resources.Load("MiniMapIcon"), SceneData.Inst.miniMap) as GameObject).GetComponent<MiniMapIcon>();
        icon.Initialize(transform, Color.red);
        deadAction += () => Destroy(icon.gameObject);

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
        MoveToPos(pos, ()=> StartCoroutine(Roaming(Random.Range(1.0f,3.0f))));  // 기존의 MoveToPos 함수 사용하는데, 델리게이트 (UnityAction) 사용해서
                                                                                // MoveToPos를 재호출 
    }

    public void Find(Transform target)
    {
        myTarget = target;
        myTarget.GetComponent<CharacterProperty>().DeathAlarm += () => { if (IsLive) ChangeState(State.Normal); }; // 플레이어를 공격하던 대상을 노말상태로 만든다
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
        //        Destroy(this.gameObject, 1.5f); // 리팩토링이 불가능 -> 비효율적
        //    }
        //    yield return null;
        //}


        // 죽었을 때 딜레이를 갖고
        yield return new WaitForSeconds(3.0f);

        // 일정 높이 (y) 이하로 내려가다가 해당 오브젝트 파괴
        float dist = 0.0f;
        while(dist < 1.0f)
        {
            dist += Time.deltaTime;
            transform.Translate(Vector3.down * 0.5f * Time.deltaTime);
            yield return null;
        }
        deadAction?.Invoke();       // UI icon과  
        Destroy(this.gameObject);
        TotalCount--;
    }
}
