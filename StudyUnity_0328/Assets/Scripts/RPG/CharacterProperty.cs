using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterProperty : MonoBehaviour
{
    public UnityAction DeathAlarm;

    public float MoveSpeed = 2.0f;
    public float RotSpeed = 360.0f;
    public float AttackRange = 1.0f;
    public float AttackDelay = 1.0f;
    protected float playTime = 0.0f;
    
    public float MaxHp = 100.0f;
    float _curHp = -100.0f;
    public float AttackPoint = 35.0f;

    protected float curHp 
    {
        get
        {
            if (_curHp < 0.0f) _curHp = MaxHp;
            return _curHp;
        }
        set => _curHp = Mathf.Clamp(value, 0.0f, MaxHp);
    }

    Animator _anim = null;
    protected Animator myAnim
    {
        get
        {
            if(_anim == null)
            {
                _anim = GetComponent<Animator>();
                if(_anim == null)
                {
                    _anim = GetComponentInChildren<Animator>();
                }
            }
            return _anim;
        }
    }
}
