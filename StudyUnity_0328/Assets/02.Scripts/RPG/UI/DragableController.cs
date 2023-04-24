using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 dragOffset = Vector2.zero; // 드래그 되기 전 위치부터 이동할 거리를 구하기 위한 오프셋
    Vector2 halfSize = Vector2.zero;

    void Start()
    {
        halfSize = transform.parent.GetComponent<RectTransform>().sizeDelta * 0.5f; // 부모의 Anchor sizeDelta(width, height) 절반 값을 미리 저장
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragOffset = (Vector2)transform.parent.position - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = eventData.position + dragOffset;

        pos.x = Mathf.Clamp(pos.x, halfSize.x, Screen.width - halfSize.x);  // 현재 화면의 폭, 스크린의 원점은 좌측 하단
        pos.y = Mathf.Clamp(pos.y, halfSize.y, Screen.height - halfSize.y); // 현재 화면의 높이

        transform.parent.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
