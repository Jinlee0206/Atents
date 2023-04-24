using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DragItem newItem = eventData.pointerDrag.GetComponent<DragItem>();
        DragItem curItem = GetComponentInChildren<DragItem>();
        if(curItem != null)
        {
            curItem.ChangeParent(newItem.orgParent, true);
        }
        newItem?.ChangeParent(this.transform); // 이벤트데이터(마우스포인터)의 게임오브젝트 안의 DragItem 스크립트가 있다면 부모 변경 함수 실행

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
