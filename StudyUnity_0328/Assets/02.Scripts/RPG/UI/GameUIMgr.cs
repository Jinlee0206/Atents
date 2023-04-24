using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

// 싱글톤 패턴 : 씬에 단 하나의 인스턴스만 허용하는 방식
// GameUIMgr는 GameMgr에서 인스턴스 생성해서 사용하는 방식을 쓸 것
public class GameUIMgr : MonoBehaviour
{
    static GameUIMgr _inst = null;

    public static GameUIMgr Inst
    {
        get
        {
            if(_inst == null)
            {
                _inst = FindObjectOfType<GameUIMgr>();
                if(_inst == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "GameUIMgr";
                    _inst = obj.AddComponent<GameUIMgr>();
                }
            }
            return _inst;
        }
    }

    private void Awake()
    {
        GameUIMgr inst = FindObjectOfType<GameUIMgr>();
        if(inst != this)
        {
            Destroy(this);
            return;
        }
        stringBuilder = new StringBuilder();
    }

    public StringBuilder stringBuilder = null;

    public string MergeChar(string a, char b)
    {
        stringBuilder.Clear();
        stringBuilder.Append(a);
        stringBuilder.Append(b);
        return stringBuilder.ToString();
    }

    public string MergeString(string a, string b)
    {
        stringBuilder.Clear();
        stringBuilder.Append(a);
        stringBuilder.Append(b);
        return stringBuilder.ToString();
    }


}
