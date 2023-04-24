using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 자신의 UI(슬라이더 등)을 제어하기 위한 스크립트
public class PlayerStatUI : MonoBehaviour
{
    public Slider myHPBar;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateHP(float v)
    {
        myHPBar.value = v;
    }
}
