using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Drag&Drop 구현을 위해 필요한 인터페이스 상속받는다
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    Vector2 dragOffset = Vector2.zero;
    public Transform orgParent { get; private set; }

    public Image myIcon = null;
    public float coolTime = 3.0f;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.clickCount == 2)
        {
            if (Mathf.Approximately(myIcon.fillAmount, 1.0f))
            {
                myIcon.fillAmount = 0.0f;
                StopAllCoroutines();
                StartCoroutine(Cooling());
            }
        }
    }

    IEnumerator Cooling()
    {
        // y : 1.0f = x : coolTime;
        // y = x / coolTime;
        float speed = 1.0f / coolTime;
        while(myIcon.fillAmount < 1.0f)
        {
            myIcon.fillAmount += speed * Time.deltaTime;
            yield return null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragOffset = (Vector2)transform.position - eventData.position;
        orgParent = transform.parent;
        transform.SetParent(transform.parent.parent); // 원래 부모와 동등하게 변경, 하이어라키상 부모와 같은 위치 -> 인벤토리 슬롯을 가리지 않음 
        transform.SetAsLastSibling(); // 로컬 트랜스폼의 가장 아래쪽으로 이동시킨다
        GetComponent<Image>().raycastTarget = false; // 이미지가 있다면 Graphic의 레이캐스트 타겟을 false로 변경
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + dragOffset; // 해당 이벤트데이터 포지션(마우스 포지션) 값을 실제 이미지 포지션으로 변경
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(orgParent);
        //transform.position = orgPos;
        transform.localPosition = Vector3.zero;
        GetComponent<Image>().raycastTarget = true;
    }

    //Vector3 orgPos = Vector3.zero;

    void Start()
    {
        //orgPos = transform.position;

    }

    public void ChangeParent(Transform p, bool update = false)
    {
        orgParent = p;
        if(update)
        {
            transform.SetParent(p);
            transform.localPosition = Vector3.zero;
        }
    }

}
