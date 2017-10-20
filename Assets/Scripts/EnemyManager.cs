using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    static public EnemyManager instance;

    public List<Transform> wayPoints = new List<Transform>();
    public List<GameObject> enemyList = new List<GameObject>();

    public GameObject enemyPrefab;
    private int enemyPrefabNum = 70;

    public int spawnNum { get; set; }           //스폰된수
    public int spawnMax { get; set; }           //스폰될수(최대 스폰 수)
    public int curEnemyNum { get; set; }        //현재 활성화 되있는 수
    public int limitEnemyNum { get; set; }      //적 한계수(돌파시 GameOver)
    private float spawnTime = 0.2f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    //적 프리팹 생성
    public void MakeEnemy()
    {
        //이미 enemy들이 있다면 지워줌.
        if (transform.FindChild("enemyHolder"))
        {
            enemyList.Clear();
            
            Destroy(transform.FindChild("enemyHolder").gameObject);
        }

        //홀더 생성
        Transform enemyHolder = new GameObject("enemyHolder").transform;
        enemyHolder.parent = transform;

        for (int i = 0; i < enemyPrefabNum; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefab);
            newEnemy.name = "enemy";
            newEnemy.transform.parent = enemyHolder;
            newEnemy.SetActive(false);

            enemyList.Add(newEnemy);
        }
    }

    //적 스폰
    public IEnumerator SpawnEnemy()
    {
        while (GameManager.instance.wave)
        {
            GameObject newEnemy = GetEnemy();
            newEnemy.transform.position = wayPoints[0].position;
            newEnemy.GetComponent<Enemy>().SetEnemy(0);
            newEnemy.SetActive(true);

            spawnNum++;
            curEnemyNum++;

            //적 숫자 체크
            CheckEnemyNum(curEnemyNum, limitEnemyNum);

            yield return new WaitForSeconds(spawnTime);
        }
    }

    private void CheckEnemyNum(int _cur, int _limit)
    {
        if (_cur >= _limit)
            GameManager.instance.checkEnemy = true;
    }

    private GameObject GetEnemy()
    {
        foreach (GameObject enemy in enemyList)
        {
            if (!enemy.activeInHierarchy)
                return enemy;
        }

        Debug.LogError("no enemy in list!");
        return null;
    }
}
