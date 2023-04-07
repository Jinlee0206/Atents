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
        // 내비게이션 시스템으로 계산한 경로
        NavMeshPath path = new NavMeshPath();
        // 두 지점 사이의 거리를 계산하고 결과를 불 값으로 리턴한다
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
