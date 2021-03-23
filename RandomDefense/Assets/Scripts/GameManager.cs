using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    static public GameManager instance;

    public enum GameState
    {
        wait,
        wave,
        over,
        none
    }
    public GameState curGameState;

    public MapGenerator    myMapGenerator;
    EnemyManager    myEnemyManager;
    UnitManager     myUnitManager;
 
    public bool wave;
    public bool checkEnemy = false;

    public int waveNum;
    public int waveMax;
    public float waveTime;

    public class EventManager
    {
        public delegate void MyEvent();

        public MyEvent eGameOver;

    }
    public EventManager myEvents = new EventManager();

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        myMapGenerator = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
        myEnemyManager = EnemyManager.instance;
        myUnitManager = UnitManager.instance;

        myEvents.eGameOver += GameOver;

        InitGame();
    }
    
    //게임오버
    private void GameOver()
    {
        Debug.Log("GameOver!");

        InitGame();
        checkEnemy = false;
    }

    //웨이브 시작
    private void StartNextWave()
    {
        ChangeGameState(GameState.wave);
        
        wave = true;
        waveNum++;
        myEnemyManager.spawnNum = 0;
        myEnemyManager.spawnMax = 10;

        StartCoroutine(myEnemyManager.SpawnEnemy());            //적 스폰 시작
        myUnitManager.SpawnUnit(myMapGenerator.GetTilePos());    //유닛 스폰
    }

    //초기 게임세팅
    public void InitGame()
    {
        myMapGenerator.GenerateMap();       //맵생성
        myEnemyManager.MakeEnemy();         //적생성
        myUnitManager.MakeUnit();

        waveNum = 0;
        waveMax = 10;
        waveTime = 30f;

        myEnemyManager.curEnemyNum = 0;
        myEnemyManager.limitEnemyNum = 55;
        myUnitManager.curUnitNum = 0;

        wave = false;
    }

    //State변경
    private void ChangeGameState(GameState _state)
    {
        if (curGameState == _state)
            return;

        curGameState = _state;
    }

    private void WaitState()
    {
        if (Input.GetMouseButton(0))
        {
            StartNextWave();
        }
    }
    private void WaveState()
    {       
        //게임오버조건.
        if (checkEnemy)
        {
            ChangeGameState(GameState.over);
        }

        //스폰최대까지 스폰
        if (myEnemyManager.spawnNum >= myEnemyManager.spawnMax)
        {
            wave = false;
        }

        //wave시간마다 스폰
        if (waveTime >= 0f)
            waveTime -= Time.deltaTime;
        else
        {
            StartNextWave();
            waveTime = 30f;
        }
    }
    private void OverState()
    {
        if (checkEnemy)
        {
            myEvents.eGameOver();
        }
    }

    private void UpdateGameState()
    {
        switch (curGameState)
        {
            case GameState.wait:
                WaitState();
                break;
            case GameState.wave:
                WaveState();
                break;
            case GameState.over:
                OverState();
                break;
            case GameState.none:
                break;
        }
    }

    public void Update()
    {
        UpdateGameState();
    }
}
