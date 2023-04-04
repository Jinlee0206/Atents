using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  IPerception ����� ��Ų ��, �ڽ� Ŭ�������� �������̵� �ؼ� ����ϱ� ���ؼ� interface�� ����
public interface IPerception
{
    void Find(Transform target);
    void LostTarget();
}

// Monster�� ���� ������Ʈ�� AIPerception �� ���� ������Ʈ ����
public class AIPerception : MonoBehaviour
{
    public LayerMask enemyMask;                                     // ���̾� ����ũ : ��(�÷��̾�)
    public List<Transform> myEnemylist = new List<Transform>();     // �� OnTriggerEnter �� ���� ����Ʈ ����
    IPerception myParent = null;                                    // IPerception interface �� myParent �ν��Ͻ� ���� 
                                                                    // myParent Ÿ���� �������̵� �� �Լ� ������ ����
    Transform myTarget = null;                                      // myTarget Ʈ������ �ν��Ͻ� ����

    void Start()
    {
        myParent = transform.parent.GetComponent<IPerception>();    // myParent�� �� ��ũ��Ʈ�� �پ��ִ� Ʈ�������� �θ𿡼� IPerception ������Ʈ�� �����ͼ� ����
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // ������ ���� ����ũ�� Trigger�� �ߵ���Ų other�� ����ũ�� 0�� �ƴ� �� (�� ������ ������ �ν����� ����)
        if((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            if (!myEnemylist.Contains(other.transform)) // ����Ʈ�� ��� ���� ���� ���̸�
            {
                myEnemylist.Add(other.transform); // ����Ʈ�� Ÿ�� �ֱ�
            }

            if(myTarget == null)                  // �� Ÿ�� ���� ���̸�
            {
                myTarget = other.transform;       // �浹�� �ݶ��̴��� Ʈ�������� �� Ÿ������ ����
                myParent.Find(myTarget);          // �θ� �ִ� �������̵��� Find �Լ� ���� 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            if (myEnemylist.Contains(other.transform))
            {
                myEnemylist.Remove(other.transform); // ����Ʈ���� Ÿ�� ����
            }

            if(myTarget == other.transform)         
            {
                myTarget = null;
                myParent.LostTarget();
            }
        }
    }
}
