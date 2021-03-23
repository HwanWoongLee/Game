using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class UIManager : MonoBehaviour
{
    public UILabel curRecord;
    public UILabel topRecord;
    public UILabel ballCount;
    public UILabel coinCount;

    public UIButton doubleButton;
    public UIButton pauseButton;
    public UIButton replayButton;

    public GameObject chancePop;
    public GameObject endPop;
    public GameObject pausePop;
    public GameObject board;
    public GameObject helpPop;
    public GameObject backButton;
    
    [SerializeField]
    bool pause;

    bool click1;
    float delayTime;

    bool escape;
    public GameObject escapePop;

    private void Start()
    {
        //광고 ID.
        //Advertisement.Initialize("1305491");

        pause = false;
        click1 = false;
        delayTime = 0f;
    }

    void Update()
    {
        //기록표시.
        curRecord.text = "" + GameManager.instance.curWave.ToString();
        topRecord.text = "TOP " + GameManager.instance.topWave.ToString();
        coinCount.text = GameManager.instance.coin.ToString();

        //볼개수표시.
        ballCount.transform.position = new Vector3(BallManager.instance.setPos.x, -3.8f, 0f);
        ballCount.text = "x" + GameManager.instance.ballNum;

        //UI상태들.
        UIState(GameManager.instance.curGameState);

        //홈버튼 클릭.
        if (click1)
        {
            delayTime += Time.deltaTime;
            if(delayTime >= .3f)
            {
                Application.LoadLevel("MainScene");
                delayTime = 0f;
                click1 = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!escape)
            {
                escapePop.GetComponent<TweenScale>().ResetToBeginning();
                escapePop.GetComponent<TweenScale>().enabled = true;
                escapePop.SetActive(true);

                Time.timeScale = 0f;

                escape = true;
            }
            else if (escape)
            {
                escapePop.SetActive(false);

                Time.timeScale = 1f;

                escape = false;
            }
        }
    }

    public void escapeNo()
    {
        SoundManager.instance.PlaySound(0);

        Application.Quit();
 
    }
    public void escapeYes()
    {
        SoundManager.instance.PlaySound(0);
        escapePop.SetActive(false);

        Time.timeScale = 1f;

        escape = false;

    }

    void UIState(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.moveblock:
                ballCount.transform.rotation = Quaternion.RotateTowards(ballCount.transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * 500f);
                doubleButton.gameObject.SetActive(false);

                chancePop.SetActive(false);
                endPop.SetActive(false);
                break;
            case GameManager.GameState.ready:
                ballCount.transform.rotation = Quaternion.RotateTowards(ballCount.transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * 500f);
                doubleButton.gameObject.SetActive(false);

                chancePop.SetActive(false);
                endPop.SetActive(false);

                //pauseButton.gameObject.SetActive(true);
                break;
            case GameManager.GameState.fire:
                ballCount.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                doubleButton.gameObject.SetActive(true);

                chancePop.SetActive(false);
                endPop.SetActive(false);
                break;
            case GameManager.GameState.gameover:
                if (chancePop != null)
                    chancePop.SetActive(true);

                ballCount.transform.rotation = Quaternion.RotateTowards(ballCount.transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * 500f);
                doubleButton.gameObject.SetActive(false);
                pauseButton.gameObject.SetActive(false);
                break;
            case GameManager.GameState.end:
                if (endPop != null)
                {
                    endPop.SetActive(true);
                }
                if (chancePop != null)
                    chancePop.SetActive(false);

                ballCount.transform.rotation = Quaternion.RotateTowards(ballCount.transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * 500f);
                doubleButton.gameObject.SetActive(false);

                pauseButton.gameObject.SetActive(false);
                break;
        }
    }

    void BallCountRotate()
    {
        ballCount.transform.rotation = Quaternion.RotateTowards(Quaternion.Euler(0f, 90f, 0f), Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * 500f);
    }

    //한번더?클릭
    public void ClickOneMoreChance()
    {
        SoundManager.instance.PlaySound(0);

        //데이터저장.
        DataManager.Instance.SaveData();

        //아래 3단을 지워준다.
        BlockManager.instance.Search();
        foreach(GameObject block in BlockManager.instance.findBlocks)
        {
            if(block.transform.position.y <= -1f)
            {
                block.SetActive(false);
            }
        }

        //게임상태 바꿔줌.
        GameManager.instance.gameOverCheck = 0;
        GameManager.instance.curGameState = GameManager.GameState.moveblock;
        BlockManager.instance.InitBlock();
        BlockManager.instance.ArrangeBlock();           //블록배치.
        ItemManager.instance.ArrangeItem();             //아이템배치.
        GameManager.instance.curWave++;

        //pop창 닫음.
        if (chancePop != null)
            chancePop.SetActive(false);
    }

    //OUT 클릭.
    public void ClickOut()
    {
        SoundManager.instance.PlaySound(0);

        DataManager.Instance.SaveData();
        DataManager.Instance.InitData();

        GameManager.instance.curGameState = GameManager.GameState.end;
    }

    //2배속 클릭.
    public void ClickDownDouble()
    {
        if(GameManager.instance.curGameState == GameManager.GameState.fire
            && !pause)
            Time.timeScale = 2f;
    }
    public void ClickUpDouble()
    {
        if (GameManager.instance.curGameState == GameManager.GameState.fire
           && !pause)
            Time.timeScale = 1f;
    }

    //일시정지 클릭.
    public void PauseButtonClick()
    {
        SoundManager.instance.PlaySound(0);

        pause = true;

        board.GetComponent<BoxCollider>().enabled = false;
        Time.timeScale = 0f;
        pauseButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(true);
        pausePop.SetActive(true);
        doubleButton.gameObject.SetActive(false);
    }

    //재시작 클릭.
    public void ReplayButtonClick()
    {
        SoundManager.instance.PlaySound(0);

        pause = false;

        board.GetComponent<BoxCollider>().enabled = true;
        Time.timeScale = 1f;
        pauseButton.gameObject.SetActive(true);
        replayButton.gameObject.SetActive(false);
        pausePop.SetActive(false);
        doubleButton.gameObject.SetActive(true);

    }

    //홈버튼 클릭.
    public void ClickHomeButton()
    {
        SoundManager.instance.PlaySound(0);

        Fade.instance.curFadeState = Fade.FadeState._out;

        DataManager.Instance.term = 0;

        DataManager.Instance.SaveData();
        DataManager.Instance.InitData();

        click1 = true;
    }

    //도움말 클릭.
    public void HelpButtonClick()
    {
        SoundManager.instance.PlaySound(0);

        helpPop.SetActive(true);
        replayButton.gameObject.SetActive(false);
        backButton.SetActive(true);
    }
   
    //back버튼 클릭.
    public void BackButtonClick()
    {
        SoundManager.instance.PlaySound(0);

        helpPop.SetActive(false);
        replayButton.gameObject.SetActive(true);
        backButton.SetActive(false);
    }

    //////////////광고////////////////////
    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            GameManager.instance.oneMoreCheck = 1;      //광고찬스 체크.

            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    public void ShowAd()
    {
        if (GameManager.instance.oneMoreCheck == 2)
            return;

        if (Advertisement.IsReady("rewardedVideo"))
        {
            GameManager.instance.oneMoreCheck = 2;

            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                //광고를 다보면.
                Debug.Log("The ad was successfully shown.");
                if (GameManager.instance.oneMoreCheck == 1)
                {
                    ClickOneMoreChance();
                }
                else if (GameManager.instance.oneMoreCheck == 2)
                {
                    GameManager.instance.AdsGetCoin();
                }

                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}
