using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProperty2D : CharacterProperty
{
    SpriteRenderer _renderer = null;        // 2D���� ����ϴ� Renderer Ÿ��

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

    // Rigidbody2D�� Rigidbody���� ��ӹ޾Ƽ� �����Ǿ� �ֱ� ������ CharacterProperty ��� �ֻ��� parent ��ũ��Ʈ�� �����ϰ�
    // ���� Rigidbody2D���� �ִ� �޼ҵ带 ����ϱ� ���ؼ��� as�� �̿��� ����ȯ(�ٿ�ĳ����) �Ͽ��� ����Ѵ�
    // �ٿ� ĳ���� : �θ� Ŭ������ ��ĳ���õ� �ڽ� Ŭ������ �����Ͽ� ������ �ʵ�� ����� ȸ���ϱ� ����
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
