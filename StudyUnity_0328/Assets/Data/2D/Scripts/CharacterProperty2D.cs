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

    // Rigidbody2D는 Rigidbody에게 상속받아서 구현되어 있기 때문에 CharacterProperty 라는 최상위 parent 스크립트에 선언하고
    // 만약 Rigidbody2D에만 있는 메소드를 사용하기 위해서는 as를 이용해 형변환(다운캐스팅) 하여서 사용한다
    // 다운 캐스팅 : 부모 클래스로 업캐스팅된 자식 클래스를 복구하여 본인의 필드와 기능을 회복하기 위함
    Rigidbody2D _rigid2D = null;
    protected Rigidbody2D myRigid2D
    {
        get
        {
            if (_rigid2D == null)
            {
                _rigid2D = GetComponent<Rigidbody2D>();
                if (_rigid2D == null)
                {
                    _rigid2D = GetComponentInChildren<Rigidbody2D>();
                }
            }
            return _rigid2D;
        }

    }


}
