using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �ڽ��� UI(�����̴� ��)�� �����ϱ� ���� ��ũ��Ʈ
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
