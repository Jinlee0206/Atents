using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 dragOffset = Vector2.zero; // �巡�� �Ǳ� �� ��ġ���� �̵��� �Ÿ��� ���ϱ� ���� ������
    Vector2 halfSize = Vector2.zero;

    void Start()
    {
        halfSize = transform.parent.GetComponent<RectTransform>().sizeDelta * 0.5f; // �θ��� Anchor sizeDelta(width, height) ���� ���� �̸� ����
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragOffset = (Vector2)transform.parent.position - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = eventData.position + dragOffset;

        pos.x = Mathf.Clamp(pos.x, halfSize.x, Screen.width - halfSize.x);  // ���� ȭ���� ��, ��ũ���� ������ ���� �ϴ�
        pos.y = Mathf.Clamp(pos.y, halfSize.y, Screen.height - halfSize.y); // ���� ȭ���� ����

        transform.parent.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
