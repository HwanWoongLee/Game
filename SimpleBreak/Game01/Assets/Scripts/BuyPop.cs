using UnityEngine;
using System.Collections;

public class BuyPop : MonoBehaviour {
    public UISprite image;
    public UILabel nCoin;

    public ShopUIManager shop;

    public GameObject backButton;

    public GameObject fire, eraser, blackhole,coin;
    public GameObject Message;

    // Update is called once per frame
    void Update () {
        switch (shop.type)
        {
            case 1:
                coin.SetActive(false);
                fire.SetActive(true);
                eraser.SetActive(false);
                blackhole.SetActive(false);
                nCoin.text = ((PlayerPrefs.GetInt("FIREPROB")+1) * 50).ToString();
                break;
            case 2:
                coin.SetActive(false);
                fire.SetActive(false);
                eraser.SetActive(false);
                blackhole.SetActive(true);
                nCoin.text = ((PlayerPrefs.GetInt("POTALPROB")+1) * 100).ToString();
                break;
            case 3:
                coin.SetActive(false);
                fire.SetActive(false);
                eraser.SetActive(true);
                blackhole.SetActive(false);
                nCoin.text = ((PlayerPrefs.GetInt("ERASERPROB")+1) * 50).ToString();
                break;
            case 4:
                coin.SetActive(true);
                fire.SetActive(false);
                eraser.SetActive(false);
                blackhole.SetActive(false);
                nCoin.text = (((PlayerPrefs.GetInt("COINPROB")) * 10) + 50).ToString();
                break;
        }

        Debug.Log("fire" + DataManager.Instance.fireProb);
        Debug.Log("eraser" + DataManager.Instance.eraserProb);
        Debug.Log("coin" + DataManager.Instance.coinProb);
        Debug.Log("potal" + DataManager.Instance.potalProb);

    }

    public void YesButton()
    {
        switch (shop.type)
        {
            case 1:
                if (PlayerPrefs.GetInt("COIN") >= (PlayerPrefs.GetInt("FIREPROB") + 1) * 50)
                {
                    int _coin = PlayerPrefs.GetInt("COIN") - (PlayerPrefs.GetInt("FIREPROB") + 1) * 50;
                    PlayerPrefs.SetInt("COIN", _coin);

                    shop.FireUp();
                }
                else
                {
                    Message.GetComponent<TweenPosition>().ResetToBeginning();
                    Message.GetComponent<TweenPosition>().enabled = true;
                    Message.GetComponent<TweenAlpha>().ResetToBeginning();
                    Message.GetComponent<TweenAlpha>().enabled = true;
                    Message.SetActive(true);
                }
                break;
            case 2:
                if (PlayerPrefs.GetInt("COIN") >= (PlayerPrefs.GetInt("POTALPROB") + 1) * 100)
                {
                    int _coin = PlayerPrefs.GetInt("COIN") - (PlayerPrefs.GetInt("POTALPROB") + 1) * 100;
                    PlayerPrefs.SetInt("COIN", _coin);

                    shop.PotalUp();
                }
                else
                {
                    Message.GetComponent<TweenPosition>().ResetToBeginning();
                    Message.GetComponent<TweenPosition>().enabled = true;
                    Message.GetComponent<TweenAlpha>().ResetToBeginning();
                    Message.GetComponent<TweenAlpha>().enabled = true;
                    Message.SetActive(true);
                }
                break;
            case 3:
                if (PlayerPrefs.GetInt("COIN") >= (PlayerPrefs.GetInt("ERASERPROB") + 1) * 50)
                {
                    int _coin = PlayerPrefs.GetInt("COIN") - (PlayerPrefs.GetInt("ERASERPROB") + 1) * 50;
                    PlayerPrefs.SetInt("COIN", _coin);

                    shop.EraserUp();
                }
                else
                {
                    Message.GetComponent<TweenPosition>().ResetToBeginning();
                    Message.GetComponent<TweenPosition>().enabled = true;
                    Message.GetComponent<TweenAlpha>().ResetToBeginning();
                    Message.GetComponent<TweenAlpha>().enabled = true;
                    Message.SetActive(true);
                }
                break;
            case 4:
                if (PlayerPrefs.GetInt("COIN") >= (PlayerPrefs.GetInt("COINPROB") + 1) * 50)
                {
                    int _coin = PlayerPrefs.GetInt("COIN") - (PlayerPrefs.GetInt("COINPROB") + 1) * 50;
                    PlayerPrefs.SetInt("COIN", _coin);

                    shop.CoinUp();
                }
                else
                {
                    Message.GetComponent<TweenPosition>().ResetToBeginning();
                    Message.GetComponent<TweenPosition>().enabled = true;
                    Message.GetComponent<TweenAlpha>().ResetToBeginning();
                    Message.GetComponent<TweenAlpha>().enabled = true;
                    Message.SetActive(true);
                }
                break;
        }
        this.gameObject.SetActive(false);
        backButton.SetActive(true);
    }

    public void NoButton()
    {
        this.gameObject.SetActive(false);
        backButton.SetActive(true);

    }
}
