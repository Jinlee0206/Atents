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
        newItem?.ChangeParent(this.transform); // �̺�Ʈ������(���콺������)�� ���ӿ�����Ʈ ���� DragItem ��ũ��Ʈ�� �ִٸ� �θ� ���� �Լ� ����

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
