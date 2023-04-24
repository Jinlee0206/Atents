using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour
{
    public void MoveBox(float v)
    {
        transform.position = orgPos + Vector3.right * v;
    }

    Vector3 orgPos = Vector3.zero;


    void Start()
    {
        orgPos = transform.position;    
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            PopUpManager.Inst.CreatePopUp("테스트", "내용없음");
        }

        
    }
}
