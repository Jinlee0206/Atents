using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{
    public Transform myRoot;
    public Slider mySlider;

    public void updateHP(float v)
    {
        mySlider.value = v;
    }

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(myRoot.position); // root의 월드 상의 좌표가 스크린 상의 좌표로 변형
        if (transform.position.z < 0.0f)
        {
            transform.position += Vector3.up * 10000.0f;
            //this.gameObject.SetActive(false);
        }
    }
}
