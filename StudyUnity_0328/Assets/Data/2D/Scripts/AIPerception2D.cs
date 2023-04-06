using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;   

public class AIPerception2D : MonoBehaviour
{
    public LayerMask enemyMask;
    CircleCollider2D myCircleCollider2D;

    public UnityEvent<Transform> FindEnemy; // 관계를 느슨하게 하기위해 유니티 이벤트 사용해서 delegate 함수를 만들고 인스펙터 창에서 binding 

    private void Start()
    {
        StartCoroutine(Searching());        // 시작할 때 Searching 코루틴 1회 시작
    }

    private void Awake()
    {
        myCircleCollider2D = GetComponent<CircleCollider2D>();
    }

    // 물리 엔진 처리를 위한 함수
    void FixedUpdate()
    {

    }

    IEnumerator Searching()
    {
        Collider2D col = null;
        while(!col)
        {
            col = Physics2D.OverlapCircle(transform.position, myCircleCollider2D.radius, enemyMask);  // Raycast와 비슷한 원리. 가상의 구를 만들어 곂치는 콜라이더가 있는지 검사한다
            if (col != null)
            {
                FindEnemy?.Invoke(col.transform);
            }
            yield return new WaitForFixedUpdate();  // FixedUpdate 프레임 변화에 맞게 코루틴 동작
                                                    // 적 찾게되면 코루틴 종료
        }
    }

}
