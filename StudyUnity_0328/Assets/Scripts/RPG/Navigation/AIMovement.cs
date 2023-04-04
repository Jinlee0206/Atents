using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : CharacterMovement
{
    
    protected void MoveByPath(Vector3[] pathList)
    {
        StopAllCoroutines();
        StartCoroutine(MovingByPath(pathList));
    }

    IEnumerator MovingByPath(Vector3[] pathList)
    {
        int i = 1;
        myAnim.SetFloat("Speed", 1.0f);
        while(i < pathList.Length)
        {
            bool done = false;
            MoveToPos(pathList[i], () => done = true); // UnityAction -> 코루틴 함수가 끝날 때 실행되는 함수
            while(!done)
            {
                for(int j = i; j<pathList.Length; j++)
                {
                    Debug.DrawLine(j == 1 ? transform.position : pathList[j-1], pathList[j]);
                }
                yield return null;
            }
            i++;
        }
        myAnim.SetFloat("Speed", 0.0f);
    }
}
