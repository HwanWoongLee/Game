using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public enum GameState { ready, fire, moveblock, gameover, end, none}

    public GameState curGameState = GameState.ready;

    public static GameManager instance;

    public int ballNum;     //볼개수.
    public int curWave;     //현제기록.
    public int topWave;     //최고기록.
    public int coin;        //코인.

    public int gameOverCheck = 0;
    public int oneMoreCheck = 0;

    string gameData;
    string[] map;

    bool soundCheck = false;

    void Awake()
    {
        if (instance == null)
            instance = this;

        //초기화.
        //DataManager.Instance.InitData();

        //게임실행이 처음이라면.
        if (DataManager.Instance.FirstDecide())
        {
            Debug.Log("처음실행.");

            curWave = 1;
            topWave = PlayerPrefs.GetInt("TOPWAVE");
            ballNum = 1;
            coin = PlayerPrefs.GetInt("COIN");
        }
        else
        {
            Debug.Log("처음실행이 아니다.");

            //게임 실행이 처음이 아닌경우.
            DataManager.Instance.LoadData();

        }
        DataManager.Instance.term = 1;

    }

    private void Start()
    {
        SoundManager.instance.PlaySound(9);
    }
    public void AddWave()
    {
        curWave++;
        if (topWave <= curWave)
            topWave = curWave;
    }

    public void AddBall()
    {
        SoundManager.instance.PlaySound(4);

        ballNum++;
    }

    public void AddCoin()
    {
        SoundManager.instance.PlaySound(5);

        coin++;
    }

    //광고 코인(상점 아이템에 맞춰서 코인 획득.)
    public void AdsGetCoin()
    {
        int[] arr = new int[4];

        arr[0] = PlayerPrefs.GetInt("FIREPROB");
        arr[1] = PlayerPrefs.GetInt("ERASERPROB");
        arr[2] = PlayerPrefs.GetInt("COINPROB");
        arr[3] = PlayerPrefs.GetInt("POTALPROB");

        int getCoin = arr[0];
        int temp = 0;

        for (int i = 0; i < 4; i++)
        {
            if (arr[i] < getCoin)
            {
                getCoin = arr[i];
                temp = i;
            }
        }

        Debug.Log(temp);

        switch (temp)
        {
            case 0:
                getCoin = (PlayerPrefs.GetInt("FIREPROB") + 1) * 50;
                break;
            case 1:
                getCoin = (PlayerPrefs.GetInt("POTALPROB") + 1) * 100;
                break;
            case 2:
                getCoin = (PlayerPrefs.GetInt("ERASERPROB") + 1) * 50;
                break;
            case 3:
                getCoin = ((PlayerPrefs.GetInt("COINPROB")) * 10) + 50;
                break;
        }

        getCoin -= 20;

        coin += getCoin;

        DataManager.Instance.SaveData();
    }

    void GameStateUpdate()
    {
        switch (curGameState)
        {
            case GameState.ready:
                ReadyState();
                break;
            case GameState.fire:
                FireState();
                break;
            case GameState.moveblock:
                MoveBlockState();
                break;
            case GameState.gameover:
                GameOverState();
                break;
            case GameState.end:
                GameEndState();
                break;
        }
    }

    void ReadyState()
    {
        if (gameOverCheck == 1 && oneMoreCheck == 0)
        {
            curGameState = GameState.gameover;
        }
        DataManager.Instance.SaveData();
        DataManager.Instance.SaveGame();
        soundCheck = false;
    }

    void FireState()
    {
        if (gameOverCheck == 1 && oneMoreCheck == 0)
        {
            curGameState = GameState.gameover;
        }
    }

    void MoveBlockState()
    {
        if (gameOverCheck == 1 && oneMoreCheck == 0)
        {
            curGameState = GameState.gameover;
        }

        //충돌 수가 던진 공의 수와 같을 시(마지막 공이 바닥에 충돌시)
        if (BallManager.instance.checkNum == BallManager.instance.throwNum)
        {
            BlockManager.instance.FallBlock();
            ItemManager.instance.FallItem();
        }
    }

    void GameOverState()
    {
        if (!soundCheck)
        {
            SoundManager.instance.PlaySound(8);
            soundCheck = true;
        }
    }

    void GameEndState()
    { 
        //랭크등록.
        Social.ReportScore(topWave, "CgkInMPoge8EEAIQAQ", success => {
            Debug.Log("report_score");
        });

        //업적등록.
        if (topWave >= 60)
        {
            Social.ReportProgress("CgkInMPoge8EEAIQAg", 100.0f, success =>
            {
                Debug.Log("report_achieve");
            });
        }
        if(topWave >= 160)
        {
            Social.ReportProgress("CgkInMPoge8EEAIQAw", 100.0f, success =>
            {
                Debug.Log("report_achieve");
            });
        }
        if (topWave >= 360)
        {
            Social.ReportProgress("CgkInMPoge8EEAIQCQ", 100.0f, success =>
            {
                Debug.Log("report_achieve");
            });
        }
        if (topWave >= 560)
        {
            Social.ReportProgress("CgkInMPoge8EEAIQBA", 100.0f, success =>
            {
                Debug.Log("report_achieve");
            });
        }
        if (topWave >= 660)
        {
            Social.ReportProgress("CgkInMPoge8EEAIQBQ", 100.0f, success =>
            {
                Debug.Log("report_achieve");
            });
        }
        if (topWave >= 1060)
        {
            Social.ReportProgress("CgkInMPoge8EEAIQBg", 100.0f, success =>
            {
                Debug.Log("report_achieve");
            });
        }
        if (topWave >= 2060)
        {
            Social.ReportProgress("CgkInMPoge8EEAIQBw", 100.0f, success =>
            {
                Debug.Log("report_achieve");
            });
        }
        if (topWave >= 6060)
        {
            Social.ReportProgress("CgkInMPoge8EEAIQCA", 100.0f, success =>
            {
                Debug.Log("report_achieve");
            });
        }
    }

void Update()
    {
        GameStateUpdate();
    }
}
