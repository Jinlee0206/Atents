using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopUpManager : MonoBehaviour
{
    public GameObject myNotAbleToTouch; // 팝업이 열릴 때 배경에 깔리는 창. 다른 오브젝트 클릭이 불가능하게 한다
    public Stack<PopUpWindow> popUpList = new Stack<PopUpWindow>();
    public UnityAction allClose = null;

    // UIMgr에 인스턴스 생성하고 사용하도록 변경 필요
    public static PopUpManager Inst
    {
        get; protected set;
    }

    private void Awake()
    {
        Inst = this;
    }

    public void CreatePopUp(string title, string content)
    {
        myNotAbleToTouch.SetActive(true);
        myNotAbleToTouch.transform.SetAsLastSibling();
        PopUpWindow scp = (Instantiate(Resources.Load("PopUp"), transform) as GameObject).GetComponent<PopUpWindow>();
        scp.Initialize(title, content);
        allClose += scp.OnClose;
        popUpList.Push(scp);
    }

    public void ClosePopUp(PopUpWindow pw)
    {
        allClose -= pw.OnClose;
        popUpList.Pop();
        if (popUpList.Count == 0)
        {
            myNotAbleToTouch.SetActive(false);
        }
        else
        {
            myNotAbleToTouch.transform.SetSiblingIndex(myNotAbleToTouch.transform.GetSiblingIndex() - 1);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            /*
            while (popUpList.Count != 0)
            {
                popUpList.Peek().OnClose(); // Peek() 
            }
            */
            allClose?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.Escape) && popUpList.Count > 0)
        {
            popUpList.Peek().OnClose();
        }
    }
}
