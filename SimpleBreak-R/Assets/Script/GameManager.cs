using UnityEngine;
using System.Collections;

public enum GameState
{
    SelectBallDirection,
    MakeBlockCard,
    SelectBlock,
    Ready,
    Fire,
    GameOver,
    GameEnd,
    None
}

public struct GameData
{
    public int wave;
    public int topWave;
    public int ballNum;
    public int coin;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState curGameState = GameState.SelectBallDirection;

    public GameData gameData;

    [SerializeField]
    private BallManager cBallManager = null;
    [SerializeField]
    private CardDeck cDeck = null;

    [SerializeField]
    private GameObject slots;
    GameObject newSlot;

    [SerializeField]
    private GameObject slotSlot;

    [SerializeField]
    private GameObject playButton;
    [SerializeField]
    private GameObject doubleButton;
    [SerializeField]
    private GameObject pauseButton;
    [SerializeField]
    private GameObject restartButton;

    [SerializeField]
    private GameObject gameOverPop;
    [SerializeField]
    private GameObject gameEndPop;
    [SerializeField]
    private GameObject pausePop;
    [SerializeField]
    private GameObject ByePop;
    public GameObject CoinObj;


    public bool byePopState = false;


    public bool readyState;
    public bool shopState = false;
    public int oneMoreChance = 0;

    private GameState tState;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        Application.targetFrameRate = 60;
        
        //first
        if (DataManager.Instance.FirstDecide())
        {
            InitGameData();

            if (cBallManager == null)
                return;
                
            cBallManager.MakeBalls();
            cBallManager.SetPos(new Vector3(0f, 4.8f, 0f));
   
            cBallManager.ActiveBall();
        }
        else if (!DataManager.Instance.FirstDecide())
        {

            DataManager.Instance.LoadData();

            if (cBallManager == null)
                return;

            for (int i = 0; i < gameData.ballNum; i++)
            {
                cBallManager.MakeBalls();
            }
            cBallManager.ActiveLoadBall();
        }
    }

    void Update()
    {
        Game();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!byePopState)
            {
                ByePop.GetComponent<TweenScale>().ResetToBeginning();
                ByePop.GetComponent<TweenScale>().Play();

                ByePop.SetActive(true);
                byePopState = true;
                
                Time.timeScale = 0f;
            }
            else
            {
                ByePop.GetComponent<ByePop>().No();

                Time.timeScale = 1f;
            }
        }
    }

    void Game()
    {
        switch (curGameState)
        {
            case GameState.SelectBallDirection:
                //볼 방향 설정
                SelectBallDirection();
                break;
            case GameState.MakeBlockCard:
                //블록카드 만들어줌
                MakeBlockCard();
                break;
            case GameState.SelectBlock:
                //블록카드 선택
                SelectBlock();
                break;
            case GameState.Ready:
                Ready();
                break;
            case GameState.Fire:
                Fire();
                break;
            case GameState.GameOver:
                GameOver();
                break;
            case GameState.GameEnd:
                GameEnd();
                break;
        }
    }

    bool lineState = false;


    void SelectBallDirection()
    {
        Time.timeScale = 1.0f;

        if (cBallManager != null)
            cBallManager.SelectBallDirection();
        if (doubleButton != null)
            doubleButton.SetActive(false);
        if (pauseButton != null)
            pauseButton.SetActive(true);

        //data save
        DataManager.Instance.SaveData();

        if (BlockManager.instance != null)
        {
            DataManager.Instance.SaveBlodck();
            DataManager.Instance.SaveItem();

            if (BlockManager.instance.warpState)
            {
                BlockManager.instance.OffWarp();
                cDeck.warpIn = false;
                cDeck.warpOut = false;
            }

            BlockManager.instance.FindActiveBlocks();

            if (!lineState)
            {
                BlockManager.instance.dLine.SetActive(false);
                foreach (GameObject _block in BlockManager.instance.findBlock)
                {
                    if (_block.transform.position.y >= 2.5f)
                    {
                        BlockManager.instance.dLine.SetActive(true);
                        lineState = true;
                    }
                }
            }
        }
    }
    void MakeBlockCard()
    {
        if (cDeck != null)
            cDeck.BlockCardMake();
        if (slots != null)
        {
            newSlot = Instantiate(slots);
            newSlot.transform.parent = slotSlot.transform;
            newSlot.transform.localPosition = Vector3.zero;
            newSlot.transform.localScale = new Vector3(1f, 1f, 1f);
            newSlot.SetActive(true);
        }
    }
    void SelectBlock()
    {
        if (cDeck != null)
            cDeck.BlockCardSelect();
    }
    void Ready()
    {
        if (playButton != null && !shopState)
            playButton.SetActive(true);
        else
            playButton.SetActive(false);

        BlockManager.instance.TouchSlotCheck();
    }
    void Fire()
    {
        lineState = false;

        if (newSlot != null)
            Destroy(newSlot);
        if (playButton != null)
            playButton.SetActive(false);
        if (doubleButton != null)
            doubleButton.SetActive(true);
    }
    void GameOver()
    {
        if (gameOverPop != null)
            gameOverPop.SetActive(true);
        if (pauseButton != null)
            pauseButton.SetActive(false);
        if (doubleButton != null)
            doubleButton.SetActive(false);

        //data save
        DataManager.Instance.SaveData();

        if (BlockManager.instance != null)
        {
            DataManager.Instance.SaveBlodck();
            DataManager.Instance.SaveItem();
        }


        Social.ReportScore(gameData.wave, "CgkIsvmw_dsVEAIQAQ", success =>
        {
            Debug.Log("report_score");
        });

        if (gameData.wave >= 60)
        {
            Social.ReportProgress("CgkIsvmw_dsVEAIQAg", 100.0f, (bool success) =>
            {
                Debug.Log("report_progress");
            });
        }
    }
    void GameEnd()
    {
        if (gameEndPop != null)
            gameEndPop.SetActive(true);
        if (pauseButton != null)
            pauseButton.SetActive(false);
        if (doubleButton != null)
            doubleButton.SetActive(false);

        CoinObj.SetActive(false);
    }

    void InitGameData()
    {
        //data초기화.
        gameData.wave = 1;
        gameData.ballNum = 1;
        gameData.coin = 0;
        gameData.topWave = PlayerPrefs.GetInt("TOPWAVE");
    }

    public void AddBall()
    {
        //ball+
        gameData.ballNum++;
    }

    public void AddWave()
    {
        //wave+1
        gameData.wave++;

        if (gameData.topWave <= gameData.wave)
        {
            gameData.topWave = gameData.wave;
        }

        if(gameData.wave == 60)
        {
            string ach_id = "SIMPLEBREAKR_ACH";

            Social.ReportProgress(ach_id,100f,(bool success) =>
                {
                    Debug.Log("good ach!");
                }
            );
        }
    }

    public void AddCoin(int _coin)
    {
        gameData.coin += _coin;
    }

    //button
    public void DoubleButtonDown()
    {
        if (curGameState == GameState.Fire)
        {
            Time.timeScale = 2.0f;
        }
    }

    public void DoubleButtonUp()
    {
        if (curGameState == GameState.Fire)
        {
            Time.timeScale = 1.0f;
        }
    }

    public void PauseButtonClick()
    {
        SoundManager.instance.PlayEffectSound(0);

        Time.timeScale = 0f;
        restartButton.SetActive(true);
        pauseButton.SetActive(false);

        tState = curGameState;
        curGameState = GameState.None;

        pausePop.GetComponent<TweenScale>().ResetToBeginning();
        pausePop.GetComponent<TweenScale>().Play();

        pausePop.SetActive(true);
        doubleButton.SetActive(false);
        playButton.SetActive(false);
    }

    public void RestartButtonClick()
    {
        SoundManager.instance.PlayEffectSound(0);

        Time.timeScale = 1f;
        restartButton.SetActive(false);
        pauseButton.SetActive(true);

        curGameState = tState;

        pausePop.SetActive(false);
    }
}
