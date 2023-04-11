using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �̱��� ����
// DodgeBombGameMgr�� �ս��� �����ϱ� ���� static ������ ����� ���� ������ ��Ȱ���Ѵ�
public class DodgeBombGameMgr : MonoBehaviour
{
    public static DodgeBombGameMgr Instance = null; // �ν��Ͻ��� ���α׷��� �����Ǿ��� �����ȴ�. ���� Instance�� ���� this�� �ʱ�ȭ �ϴ� ���� �Ұ���

    public enum State
    { 
        Create, Title, Play, GameOver
    }

    public State myState = State.Create;
    int myLife = 3;
    public int Life
    {
        get => myLife;
        set
        {
            myLife = Mathf.Clamp(value, 0, 5);
            myLifeUI.SetLife(myLife);

            if(myLife == 0)
            {
                ChangeState(State.GameOver);
            }
        }
    }

    int myScore = 0;
    public int Score
    {
        get => myScore;
        set
        {
            myScore = value;
            myScoreUI.text = myScore.ToString();
        }
    }

    public DodgePlayer myPlayer; // ������ ����, ���ε� 
    public SpaceShip myShip;
    public GameObject myTitleUI;
    public GameObject myGameOverUI;
    public LifeUI myLifeUI;
    public TMPro.TMP_Text myScoreUI;


    void Start()
    {
        ChangeState(State.Title);
    }

    void Update()
    {
        StateProcess();
    }

    void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;

        switch (myState)
        {
            case State.Title:
                myGameOverUI.SetActive(false);
                myPlayer.gameObject.SetActive(false);
                myShip.StopDrop();
                break;
            case State.Play:
                myGameOverUI.SetActive(false);
                myTitleUI.SetActive(false);
                myPlayer.gameObject.SetActive(true);
                myPlayer.SetActive(true);
                myShip.StartDrop();
                myLifeUI.SetLife(myLife);
                break;
            case State.GameOver:
                myGameOverUI.SetActive(true);
                myShip.StopDrop();
                myPlayer.SetActive(false);
                break;
        }
    }

    void StateProcess()
    {
        switch (myState)
        {
            case State.Title:
                if(Input.anyKey)
                {
                    ChangeState(State.Play);
                }
                break;
            case State.Play:
                break;
            case State.GameOver:
                break;
        }
    }

    public void OnRetry()
    {
        Score = 0;
        Life = 3;
        ChangeState(State.Play);
    }
}
