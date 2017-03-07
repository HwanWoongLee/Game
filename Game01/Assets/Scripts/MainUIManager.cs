using UnityEngine;
using System.Collections;

public class MainUIManager : MonoBehaviour {
    public UILabel topRecord;
    public UILabel coin;

    public UISprite shopSprite;

    public GameObject shop;
    public GameObject startButton;
    public GameObject shopButton;
    public GameObject logo, ball,rank, ach;

    public GameObject temp;

    bool click = false;
    bool click2 = false;
    bool click3 = false;

    float delayTime = 0f;

    bool escape;
    public GameObject escapePop;

    public GameObject facebookButton;
  
    //시작 클릭.
    public void ClickStartButton()
    {
        SoundManager.instance.PlaySound(0);

        Fade.instance.curFadeState = Fade.FadeState._out;

        DataManager.Instance.InitData();
        topRecord.gameObject.SetActive(false);
        shopButton.SetActive(false);
        startButton.SetActive(false);

        ball.SetActive(false);
        rank.SetActive(false);
  
        temp.SetActive(false);
        ach.SetActive(false);

        click3 = true;
    }


    //상점 클릭.
    public void ClickShopButton()
    {
        SoundManager.instance.PlaySound(0);

        Fade.instance.curFadeState = Fade.FadeState._out;

        topRecord.gameObject.SetActive(false);
        shopButton.SetActive(false);
        startButton.SetActive(false);
        
        ball.SetActive(false);
        rank.SetActive(false);

        facebookButton.SetActive(false);

        temp.SetActive(false);
        ach.SetActive(false);

        click = true;
    }
    public void PressShopButton()
    {
        shopSprite.GetComponent<TweenRotation>().ResetToBeginning();
        shopSprite.GetComponent<TweenRotation>().enabled = true;
    }
    public void ReleaseShopButton()
    {
        shopSprite.GetComponent<TweenRotation>().enabled = false;
    }

    //뒤로가기 버튼(상점안에서).
    public void BackButton()
    {
        SoundManager.instance.PlaySound(0);

        Fade.instance.curFadeState = Fade.FadeState._out;

        shop.SetActive(false);
   
        click2 = true;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);
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

    public void FaceBookButton()
    {
        SoundManager.instance.PlaySound(0);

        Application.OpenURL("https://www.facebook.com/60Celsius");
    }

    private void Awake()
    {
        if (PlayerPrefs.GetInt("TERM") == 1)
        {
            Application.LoadLevel("GameScene");
        }
    }

    private void Start()
    {
        Fade.instance.curFadeState = Fade.FadeState._in;

        int topNum = PlayerPrefs.GetInt("TOPWAVE");

        topRecord.text = "TOP\n" + topNum.ToString();
  
        shopSprite.GetComponent<TweenRotation>().enabled = false;

        //SHOPDATA
        DataManager.Instance.fireProb = PlayerPrefs.GetInt("FIREPROB");
        DataManager.Instance.eraserProb = PlayerPrefs.GetInt("ERASERPROB");
        DataManager.Instance.potalProb = PlayerPrefs.GetInt("POTALPROB");
        DataManager.Instance.coinProb = PlayerPrefs.GetInt("COINPROB");
    }

    private void Update()
    {
        coin.text = PlayerPrefs.GetInt("COIN").ToString();

       
        if (click)
        {
            delayTime += Time.deltaTime;

            if(delayTime >= .5f)
            {
                shop.SetActive(true);

                click = false;
                delayTime = 0f;
            }
        }
        if (click2)
        {
            delayTime += Time.deltaTime;

            if (delayTime >= .5f)
            {
                temp.SetActive(true);
                topRecord.gameObject.SetActive(true);
                shopButton.SetActive(true);
                startButton.SetActive(true);
                logo.SetActive(true);
                ball.SetActive(true);
                rank.SetActive(true);
                ach.SetActive(true);

                facebookButton.SetActive(true);

                click2 = false;
                delayTime = 0f;
            }
        }

        if (click3)
        {
            delayTime += Time.deltaTime;

            if (delayTime >= .5f)
            {
                Application.LoadLevel("GameScene");
                delayTime = 0f;
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
}
