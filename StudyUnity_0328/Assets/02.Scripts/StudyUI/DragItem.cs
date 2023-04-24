using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Drag&Drop ������ ���� �ʿ��� �������̽� ��ӹ޴´�
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
        transform.SetParent(transform.parent.parent); // ���� �θ�� �����ϰ� ����, ���̾��Ű�� �θ�� ���� ��ġ -> �κ��丮 ������ ������ ���� 
        transform.SetAsLastSibling(); // ���� Ʈ�������� ���� �Ʒ������� �̵���Ų��
        GetComponent<Image>().raycastTarget = false; // �̹����� �ִٸ� Graphic�� ����ĳ��Ʈ Ÿ���� false�� ����
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + dragOffset; // �ش� �̺�Ʈ������ ������(���콺 ������) ���� ���� �̹��� ���������� ����
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
