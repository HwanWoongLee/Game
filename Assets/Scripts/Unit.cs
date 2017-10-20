using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
    public enum UnitState
    {
        spawn,
        attack,
        attackWait
    }
    public UnitState curState = UnitState.spawn;
    public UILabel idLabel;

    public int      id;
    public int      attackPower;

    public float    attackSpeed;
    private float   attackTimer;

    private float   spawnTimer;

    private Vector3     curTargetPos;
    private GameObject  curEnemy;

    private void OnEnable()
    {
        attackPower = 1;
        attackSpeed = 1f;
        
        ChangeState(UnitState.spawn);
    }

    //상태변경
    void ChangeState(UnitState newState)
    {
        if (curState == newState)
            return;

        curState = newState;
    }

    //유닛세팅
    public void SetUnit(int _id)
    {
        this.id = _id;
        idLabel.text = id.ToString();
    }

    void SpawnState()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= 1f)
            ChangeState(UnitState.attack);
    }
    void AttackState()
    {
        //target이 없으면 return
        if (FindTarget() == null)
        {
            curEnemy = null;
            return;
        }
        else
        {   //타겟지정
            curEnemy = FindTarget();
        }
        
        attackTimer = 0f;

        //target으로 방향틀기
        //그래픽 오면 구현.

        ChangeState(UnitState.attackWait);
    }
    void AttackWaitState()
    {
        attackTimer += Time.deltaTime;

        if(attackTimer >= 1 / attackSpeed)
        {
            AttackEnemy();
            ChangeState(UnitState.attack);
        }
    }

    //공격
    public void AttackEnemy()
    {
        if (curEnemy.GetComponent<Enemy>().curEnemyState != Enemy.EnemyState.die)
        {
            curTargetPos = curEnemy.transform.position;
            curEnemy.GetComponent<Enemy>().SetEnemyAttack(this.attackPower);
        }
        else
        {
            curEnemy = null;
        }
    }
    
    //공격대상 찾기
    private GameObject FindTarget()
    {
        foreach(GameObject target in EnemyManager.instance.enemyList)
        {
            if (target.activeInHierarchy)
            {
                if(target.GetComponent<Enemy>().curEnemyState != Enemy.EnemyState.die)
                {
                    return target;
                }
            }
        }
        return null;
    }

    void UpdateState()
    {
        switch (curState)
        {
            case UnitState.spawn:
                SpawnState();
                break;
            case UnitState.attack:
                AttackState();
                break;
            case UnitState.attackWait:
                AttackWaitState();
                break;
        }
    }

    private void Update()
    {
        UpdateState();
    }
}
