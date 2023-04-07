using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    Coroutine move = null;
    public GameObject destroyEffect = null;
    // Start is called before the first frame update
    void Start()
    {        
        //move = StartCoroutine(Moving());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            //StopAllCoroutines();
            StopCoroutine(move);
        }
    }

    bool isQuitting = false;
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {        
        if(!isQuitting) Instantiate(destroyEffect, transform.position, Quaternion.identity);        
    }

    IEnumerator Moving()
    {
        float Range = 2.0f;
        Vector3 startPos, destPos;
        float t = 0.0f;
        float Dir = 1.0f;

        t = 0.5f;
        destPos = startPos = transform.position;
        startPos.x -= Range / 2.0f;
        destPos.x += Range / 2.0f;

        while (true)
        {
            t = Mathf.Clamp(t + Dir * Time.deltaTime, 0.0f, 1.0f);
            if (Mathf.Approximately(t, 0.0f) || Mathf.Approximately(t, 1.0f))
            {
                Dir *= -1.0f;
            }
            transform.position = Vector3.Lerp(startPos, destPos, t);
            yield return null;
        }
    }
}
