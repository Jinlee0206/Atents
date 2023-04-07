using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;   

public class AIPerception2D : MonoBehaviour
{
    public LayerMask enemyMask;
    CircleCollider2D myCircleCollider2D;

    public UnityEvent<Transform> FindEnemy; // ���踦 �����ϰ� �ϱ����� ����Ƽ �̺�Ʈ ����ؼ� delegate �Լ��� ����� �ν����� â���� binding 

    private void Start()
    {
        StartCoroutine(Searching());        // ������ �� Searching �ڷ�ƾ 1ȸ ����
    }

    private void Awake()
    {
        myCircleCollider2D = GetComponent<CircleCollider2D>();
    }

    // ���� ���� ó���� ���� �Լ�
    void FixedUpdate()
    {

    }

    IEnumerator Searching()
    {
        Collider2D col = null;
        while(!col)
        {
            col = Physics2D.OverlapCircle(transform.position, myCircleCollider2D.radius, enemyMask);  // Raycast�� ����� ����. ������ ���� ����� ��ġ�� �ݶ��̴��� �ִ��� �˻��Ѵ�
            if (col != null)
            {
                FindEnemy?.Invoke(col.transform);
            }
            yield return new WaitForFixedUpdate();  // FixedUpdate ������ ��ȭ�� �°� �ڷ�ƾ ����
                                                    // �� ã�ԵǸ� �ڷ�ƾ ����
        }
    }

}
