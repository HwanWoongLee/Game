using UnityEngine;
using System.Collections;

public class ShopUIManager : MonoBehaviour {
    public GameObject buyPop;
    public GameObject backButton;

    public UILabel fireCoin;
    public UILabel potalCoin;
    public UILabel eraserCoin;
    public UILabel coinCoin;

    public UISprite[] fireLevelBox;
    public UISprite[] potalLevelBox;
    public UISprite[] eraseLevelBox;
    public UISprite[] coinLevelBox;

    public int type = 0;

    public GameObject message;


    public void FireClick()
    {
        if (DataManager.Instance.fireProb <= 6)
        {
            SoundManager.instance.PlaySound(0);

            buyPop.GetComponent<TweenScale>().ResetToBeginning();
            buyPop.GetComponent<TweenScale>().enabled = true;
            buyPop.SetActive(true);
            backButton.SetActive(false);

            type = 1;
        }
    }
    public void PotalClick()
    {
        if (DataManager.Instance.potalProb <= 6)
        {
            SoundManager.instance.PlaySound(0);
            buyPop.GetComponent<TweenScale>().ResetToBeginning();
            buyPop.GetComponent<TweenScale>().enabled = true;
            buyPop.SetActive(true);
            backButton.SetActive(false);

            type = 2;
        }
    }
    public void EraseClick()
    {
        if (DataManager.Instance.eraserProb <= 6)
        {
            SoundManager.instance.PlaySound(0);
            buyPop.GetComponent<TweenScale>().ResetToBeginning();
            buyPop.GetComponent<TweenScale>().enabled = true;
            buyPop.SetActive(true);
            backButton.SetActive(false);

            type = 3;
        }
    }
    public void CoinClick()
    {
        if (DataManager.Instance.coinProb <= 6)
        {
            SoundManager.instance.PlaySound(0);
            buyPop.GetComponent<TweenScale>().ResetToBeginning();
            buyPop.GetComponent<TweenScale>().enabled = true;
            buyPop.SetActive(true);
            backButton.SetActive(false);

            type = 4;
        }
    }

    public void FireUp()
    {
        if (PlayerPrefs.GetInt("FIREPROB") <= 6)
        {
            DataManager.Instance.fireProb = PlayerPrefs.GetInt("FIREPROB");

            DataManager.Instance.fireProb++;

            PlayerPrefs.SetInt("FIREPROB", DataManager.Instance.fireProb);

            backButton.SetActive(true);

        }
    }
    public void PotalUp()
    {

        if (PlayerPrefs.GetInt("POTALPROB") <= 6)
        {
            DataManager.Instance.potalProb = PlayerPrefs.GetInt("POTALPROB");

            DataManager.Instance.potalProb++;

            PlayerPrefs.SetInt("POTALPROB", DataManager.Instance.potalProb);

            backButton.SetActive(true);

        }
    }
    public void EraserUp()
    {
        if (PlayerPrefs.GetInt("ERASERPROB") <= 6)
        {
            DataManager.Instance.eraserProb = PlayerPrefs.GetInt("ERASERPROB");

            DataManager.Instance.eraserProb++;

            PlayerPrefs.SetInt("ERASERPROB", DataManager.Instance.eraserProb);

            backButton.SetActive(true);

        }
    }
    public void CoinUp()
    {
        if (PlayerPrefs.GetInt("COINPROB") <= 6)
        {
            DataManager.Instance.coinProb = PlayerPrefs.GetInt("COINPROB");

            DataManager.Instance.coinProb++;

            PlayerPrefs.SetInt("COINPROB", DataManager.Instance.coinProb);

            backButton.SetActive(true);

        }
    }


    public void MessageFalse()
    {
        message.SetActive(false);
    }

    private void Update()
    {

        for (int i = 0; i < PlayerPrefs.GetInt("FIREPROB"); i++)
        {
            fireLevelBox[i].spriteName = "상점용버튼1";
        }
        for (int i = 0; i < PlayerPrefs.GetInt("ERASERPROB"); i++)
        {
            eraseLevelBox[i].spriteName = "상점용버튼1";
        }
        for (int i = 0; i < PlayerPrefs.GetInt("COINPROB"); i++)
        {
            coinLevelBox[i].spriteName = "상점용버튼1";
        }
        for (int i = 0; i < PlayerPrefs.GetInt("POTALPROB"); i++)
        {
            potalLevelBox[i].spriteName = "상점용버튼1";
        }


        if (PlayerPrefs.GetInt("FIREPROB") <= 6)
        {
            fireCoin.text = ((PlayerPrefs.GetInt("FIREPROB") + 1) * 50).ToString();
        }
        else
            fireCoin.text = "  MAX";

        if (PlayerPrefs.GetInt("ERASERPROB") <= 6)
        {
            eraserCoin.text = ((PlayerPrefs.GetInt("ERASERPROB") + 1) * 50).ToString();
        }
        else
            eraserCoin.text = "  MAX";

        if (PlayerPrefs.GetInt("POTALPROB") <= 6)
        {
            potalCoin.text = ((PlayerPrefs.GetInt("POTALPROB") + 1) * 100).ToString();
        }
        else
            potalCoin.text = "  MAX";

        if (PlayerPrefs.GetInt("COINPROB") <= 6)
        {
            coinCoin.text = (((PlayerPrefs.GetInt("COINPROB")) * 10) + 50).ToString();
        }
        else
            coinCoin.text = "  MAX";

    }
}
