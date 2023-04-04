using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPlayer : AIMovement
{


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnMove(Vector3 pos)
    {
        // ������̼� �ý������� ����� ���
        NavMeshPath path = new NavMeshPath();
        // �� ���� ������ �Ÿ��� ����ϰ� ����� �� ������ �����Ѵ�
        if(NavMesh.CalculatePath(transform.position, pos, NavMesh.AllAreas, path))
        {
            switch (path.status)
            {
                case NavMeshPathStatus.PathPartial:
                    break;
                case NavMeshPathStatus.PathInvalid:
                    break;
                case NavMeshPathStatus.PathComplete:
                    break;
            }
            MoveByPath(path.corners);
        }
    }
}
