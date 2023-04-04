using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  IPerception 상속을 시킨 후, 자식 클래스에서 오버라이드 해서 사용하기 위해서 interface로 선언
public interface IPerception
{
    void Find(Transform target);
    void LostTarget();
}

// Monster의 하위 컴포넌트로 AIPerception 빈 게임 오브젝트 생성
public class AIPerception : MonoBehaviour
{
    public LayerMask enemyMask;                                     // 레이어 마스크 : 적(플레이어)
    public List<Transform> myEnemylist = new List<Transform>();     // 적 OnTriggerEnter 시 담을 리스트 선언
    IPerception myParent = null;                                    // IPerception interface 형 myParent 인스턴스 선언 
                                                                    // myParent 타입의 오버라이딩 된 함수 선언을 위함
    Transform myTarget = null;                                      // myTarget 트랜스폼 인스턴스 생성

    void Start()
    {
        myParent = transform.parent.GetComponent<IPerception>();    // myParent에 이 스크립트가 붙어있는 트랜스폼의 부모에서 IPerception 컴포넌트를 가져와서 저장
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // 설정한 적의 마스크와 Trigger를 발동시킨 other의 마스크가 0이 아닐 때 (적 끼리는 적으로 인식하지 않음)
        if((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            if (!myEnemylist.Contains(other.transform)) // 리스트에 담긴 적이 없는 적이면
            {
                myEnemylist.Add(other.transform); // 리스트에 타겟 넣기
            }

            if(myTarget == null)                  // 내 타겟 값이 널이면
            {
                myTarget = other.transform;       // 충돌된 콜라이더의 트랜스폼을 내 타겟으로 설정
                myParent.Find(myTarget);          // 부모에 있는 오버라이딩된 Find 함수 실행 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            if (myEnemylist.Contains(other.transform))
            {
                myEnemylist.Remove(other.transform); // 리스트에서 타겟 제거
            }

            if(myTarget == other.transform)         
            {
                myTarget = null;
                myParent.LostTarget();
            }
        }
    }
}
