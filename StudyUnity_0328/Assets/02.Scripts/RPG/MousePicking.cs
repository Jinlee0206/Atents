using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MousePicking : MonoBehaviour
{
    public LayerMask pickMask;
    public LayerMask enemyMask;
    public UnityEvent<Vector3> clickAction = null;
    public UnityEvent<Transform> attackAction = null;
    public UnityEvent<Vector3> rightClick = null;

    void Start()
    {
        
    }

    void Update()
    {
        // UI에 마우스 커서가 올라갔을 때 하기 함수가 true 가 됨
        if(!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit, 1000.0f, pickMask | enemyMask))
            {
                if (((1 << hit.transform.gameObject.layer) & enemyMask) != 0)
                {
                    //공격
                    attackAction?.Invoke(hit.transform);
                }
                else
                {
                    //이동
                    clickAction?.Invoke(hit.point);
                }
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, pickMask))
            {
                rightClick?.Invoke(hit.point);
            }
        }
    }
}
