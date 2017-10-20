using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public enum EnemyState
    {
        wait,
        walk,
        die
    }
    public EnemyState curEnemyState = EnemyState.walk;

    public int id;
    public int curHp;

    public float moveSpeed = 3f;
    
    public Vector3 movePonit { get; set; }

    int movePointNum = 0;
    
    private void OnEnable()
    {
        moveSpeed = 3f;
        movePointNum = 0;
        SetMovePoint();
        ChangeState(EnemyState.walk);
    }

    public void SetEnemy(int _id)
    {
        this.id = _id;
        this.curHp = 2;
    }

    //데미지를 받음
    public void SetEnemyAttack(int _damage)
    {
        curHp -= _damage;
        UpdateAfterReceiveAttack();
    }

    //데미지 후처리
    private void UpdateAfterReceiveAttack()
    {
        if (curHp <= 0)
        {
            curHp = 0;
            ChangeState(EnemyState.die);
        }
    }
    
    //MovePoint 세팅
    private void SetMovePoint()
    {
        movePointNum++;

        if (movePointNum >= 4)
            movePointNum = 0;

        movePonit = EnemyManager.instance.wayPoints[movePointNum].position;
    }

    //이동함수.
    private void MoveToPoint()
    {
        if (curEnemyState != EnemyState.walk || movePonit == null)
            return;

        transform.position
            = Vector3.MoveTowards(transform.position, movePonit, moveSpeed * Time.deltaTime);

        if(this.transform.position == movePonit)
        {
            SetMovePoint();
        }
    }

    //상태변경.
    public void ChangeState(EnemyState _state)
    {
        if (curEnemyState == _state)
            return;

        curEnemyState = _state;
    }

    void UpdateState()
    {
        switch (curEnemyState)
        {
            case EnemyState.wait:
                WaitState();
                break;
            case EnemyState.walk:
                WalkState();
                break;
            case EnemyState.die:
                DieState();
                break;
        }
    }

    void WaitState()
    {

    }
    void WalkState()
    {
        MoveToPoint();
    }
    void DieState()
    {
        EnemyManager.instance.curEnemyNum--;
        gameObject.SetActive(false);
        ChangeState(EnemyState.wait);
    }

    private void Update()
    {
        UpdateState();
    }
}
