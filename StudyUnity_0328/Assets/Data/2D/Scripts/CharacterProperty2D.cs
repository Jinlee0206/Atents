using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProperty2D : CharacterProperty
{
    SpriteRenderer _renderer = null;        // 2D에서 사용하는 Renderer 타입

    protected SpriteRenderer myRenderer
    {
        get
        {
            if(_renderer == null)
            {
                _renderer = GetComponent<SpriteRenderer>();
                if(_renderer == null)
                {
                    _renderer = GetComponentInChildren<SpriteRenderer>();
                }
            }
            return _renderer;
        }
    }


}
