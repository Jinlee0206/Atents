using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public class ChatMessage : MonoBehaviour
{
    public TMPro.TMP_Text myLabel;

    public void SetMesaage(string msg, string tag = "")
    {
        float width = GetComponent<RectTransform>().rect.width;
        string tmp = string.Empty;
        string res = string.Empty;
        for(int i = 0; i< msg.Length; i++)
        {
            GameUIMgr.Inst.stringBuilder.Clear();
            GameUIMgr.Inst.stringBuilder.Append(tmp);
            GameUIMgr.Inst.stringBuilder.Append(msg[i]);
            Vector2 tmpSize = myLabel.GetPreferredValues(GameUIMgr.Inst.MergeChar(tmp, msg[i]));
            if(tmpSize.x > width)
            {
                tmp = GameUIMgr.Inst.MergeChar(tmp, '\n');
                res = GameUIMgr.Inst.MergeString(res, tmp);
                tmp = string.Empty;
            }
            tmp = GameUIMgr.Inst.MergeChar(tmp, msg[i]);
        }
        res = GameUIMgr.Inst.MergeString(res,tmp);
        GetComponent<RectTransform>().sizeDelta = myLabel.GetPreferredValues(res);
        myLabel.text = GameUIMgr.Inst.MergeString(tag, res);
    }
}
    