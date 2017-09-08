using UnityEngine;
using System.Collections;

public class ScrollCheckBox : MonoBehaviour {
    public int checkNum = 0;
    public int LootNum = 0;
    public UILabel LootLabel;
    public UILabel haveCoin;

    public GameObject effectPrefab;
    public GameObject grid;
    public GameObject homeButton;

    public GameObject buyPop;
    public UISprite buyImage;
    public UISprite buyImage2;
    public GameObject getCoinLabel;

    float buyTime;
    public bool buyState = false;

    public GameObject[] topImage;
    public GameObject[] rightImage;
    public GameObject[] leftImage;

    public GameObject[] leverImage;
    public GameObject valve;

    int getCoin = 0;
    int randNum = 0;

    int price;

    public UILabel priceLabel;

    private void Update()
    {
        priceLabel.text = price.ToString();
        LootLabel.text = LootNum.ToString();
        getCoinLabel.GetComponent<UILabel>().text = getCoin.ToString();
        haveCoin.text = GameManager.instance.coin.ToString();

        //구입버튼 눌렀을때
        if (buyState)
        {
            buyTime += Time.deltaTime;

            valve.GetComponent<TweenRotation>().enabled = true;
            homeButton.SetActive(false);
            grid.GetComponent<UIDragScrollView>().enabled = false;
            
            if (buyTime >= 5f)
            {
                SoundManager.instance.PlayEffectSound(14);
                buyPop.SetActive(true);

                valve.GetComponent<TweenRotation>().enabled = false;
                leverImage[1].SetActive(false);
                leverImage[0].SetActive(true);

                topImage[1].SetActive(false);
                topImage[0].SetActive(true);
                rightImage[1].SetActive(false);
                rightImage[0].SetActive(true);
                leftImage[1].SetActive(false);
                leftImage[0].SetActive(true);

                homeButton.SetActive(true);
                grid.GetComponent<UIDragScrollView>().enabled = true;
                buyState = false;
                buyTime = 0f;
            }
        }
    }

    public void BuyButton()
    {
        if (!buyState)
        {
            if (GameManager.instance.coin >= 100 && LootNum >= 10)
            {
                SoundManager.instance.PlayEffectSound(26);

                GameManager.instance.coin -= 100;
                GameObject effect = Instantiate(effectPrefab);
                effect.transform.position = new Vector3(0, .5f, 0f);

                randNum = Random.Range(30, 51);
                getCoin = (int)(LootNum * (randNum * 0.01f));

                leverImage[0].SetActive(false);
                leverImage[1].SetActive(true);
                topImage[0].SetActive(false);
                topImage[1].SetActive(true);
                rightImage[0].SetActive(false);
                rightImage[1].SetActive(true);
                leftImage[0].SetActive(false);
                leftImage[1].SetActive(true);

                switch (checkNum)
                {
                    case 1:
                        GameManager.instance.redLoot = 0;
                        GameManager.instance.redCoin += getCoin;
                        LootNum = 0;
                        break;
                    case 2:
                        GameManager.instance.blueLoot = 0;
                        GameManager.instance.blueCoin += getCoin;
                        LootNum = 0;
                        break;
                    case 3:
                        GameManager.instance.greenLoot = 0;
                        GameManager.instance.greenCoin += getCoin;
                        LootNum = 0;
                        break;
                }

                DataManager.Instance.SetData();

                SoundManager.instance.PlayEffectSound(11);
                buyState = true;
            }
            else
            {
                SoundManager.instance.PlayEffectSound(13);

                this.GetComponent<CameraShake>().shake = .5f;
                Debug.Log("not enough coin");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.name == "1"
            || other.transform.name == "2"
            || other.transform.name == "3")
        {
            checkNum = int.Parse(other.transform.name);
            
            other.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        }
        
        switch(other.transform.name)
        {
            case "1":
                price = 100;
                LootNum = GameManager.instance.redLoot;
                buyImage.spriteName = "enemy";
                buyImage2.spriteName = "stone";
                break;
            case "2":
                price = 150;
                LootNum = GameManager.instance.blueLoot;
                buyImage.spriteName = "enemy2";
                buyImage2.spriteName = "stone3";
                break;
            case "3":
                price = 300;
                LootNum = GameManager.instance.greenLoot;
                buyImage.spriteName = "enemy3";
                buyImage2.spriteName = "stone2";
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "1"
       || other.transform.name == "2"
       || other.transform.name == "3")
        {
            other.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
