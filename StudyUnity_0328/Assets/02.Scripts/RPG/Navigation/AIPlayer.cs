using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPlayer : AIMovement, IBattle
{
    public void OnDamage(float dmg)
    {
        curHp -= dmg;
    }

    public bool IsLive { get; }

    void Start()
    {
        MiniMapIcon icon =
            (Instantiate(Resources.Load("MiniMapIcon"), SceneData.Inst.miniMap) as GameObject).GetComponent<MiniMapIcon>();
        icon.Initialize(transform, Color.green);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            OnDamage(10.0f);
        }
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
