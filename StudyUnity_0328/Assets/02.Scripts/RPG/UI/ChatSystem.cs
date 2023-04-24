using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class ChatSystem : MonoBehaviour
{
    public enum ChatType
    {
        ��ü, �Ϲ�, ��Ƽ, �Ӹ�, ���
    }

    public Transform myContents;
    public TMP_InputField myInput;
    public Scrollbar myScroll;
    public TMP_Dropdown myMenu;

    void Start()
    {
        myMenu.value = -1;
        int count = System.Enum.GetValues(typeof(ChatType)).Length;
        for(int i = 0; i < count; i++)
        {
            GameUIMgr.Inst.stringBuilder.Clear();
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
            GameUIMgr.Inst.stringBuilder.Append(GetTypeColor((ChatType)i));
            GameUIMgr.Inst.stringBuilder.Append((ChatType)i).ToString();
            data.text = GameUIMgr.Inst.stringBuilder.ToString();
            myMenu.options.Add(data);
        }
        myMenu.value = 0;
    }

    string GetTypeColor(ChatType type)
    {
        string tmp = string.Empty;

        switch (type)
        {
            case ChatType.��ü:
                tmp = "<#ffff00>";
                break;
            case ChatType.�Ϲ�:
                tmp = "<#ffffff>";
                break;
            case ChatType.�Ӹ�:
                tmp = "<#ff00ff>";
                break;
            case ChatType.��Ƽ:
                tmp = "<#0000ff>";
                break;
            case ChatType.���:
                tmp = "<#00ff00>";
                break;
        }
        return tmp;


    }
    void Update()
    {
        
    }

    public void AddChat(string msg)
    {
        if(msg == string.Empty)
        {
            myInput.DeactivateInputField();
            return;
        }

        (Instantiate(Resources.Load("ChatMessage"), myContents) as GameObject).GetComponent<ChatMessage>().SetMesaage(msg, GetTypeColor((ChatType)myMenu.value));
        myInput.text = string.Empty;
        myInput.ActivateInputField();
        StartCoroutine(MakingZero());
    }

    IEnumerator MakingZero()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        myScroll.value = 0.0f;
    }
}
