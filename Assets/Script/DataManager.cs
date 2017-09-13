using UnityEngine;
using System.Collections;

public class DataManager
{
    private static DataManager instance;

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
                instance = new DataManager();

            return instance;
        }
    }

    public void SetData()
    {
        PlayerPrefs.SetInt("TOPSTAGE", GameManager.instance.topStageNum);
        PlayerPrefs.SetInt("TOPSCORE", GameManager.instance.topScore);
        PlayerPrefs.SetInt("COIN", GameManager.instance.coin);

        PlayerPrefs.SetInt("REDLOOT", GameManager.instance.redLoot);
        PlayerPrefs.SetInt("BLUELOOT", GameManager.instance.blueLoot);
        PlayerPrefs.SetInt("GREENLOOT", GameManager.instance.greenLoot);

        PlayerPrefs.SetInt("REDCOIN", GameManager.instance.redCoin);
        PlayerPrefs.SetInt("BLUECOIN", GameManager.instance.blueCoin);
        PlayerPrefs.SetInt("GREENCOIN", GameManager.instance.greenCoin);

        for (int i = 0; i < 7; i++)
        {
            PlayerPrefs.SetInt("REDLEVEL" + i.ToString(), GameManager.instance.redLevel[i]);
            PlayerPrefs.SetInt("BLUELEVEL" + i.ToString(), GameManager.instance.blueLevel[i]);
            PlayerPrefs.SetInt("GREENLEVEL" + i.ToString(), GameManager.instance.greenLevel[i]);
        }
    }

    public void GetData()
    {
        GameManager.instance.topStageNum = PlayerPrefs.GetInt("TOPSTAGE");
        GameManager.instance.topScore = PlayerPrefs.GetInt("TOPSCORE");
        GameManager.instance.coin = PlayerPrefs.GetInt("COIN");

        GameManager.instance.redLoot = PlayerPrefs.GetInt("REDLOOT");
        GameManager.instance.blueLoot = PlayerPrefs.GetInt("BLUELOOT");
        GameManager.instance.greenLoot = PlayerPrefs.GetInt("GREENLOOT");

        GameManager.instance.redCoin = PlayerPrefs.GetInt("REDCOIN");
        GameManager.instance.blueCoin = PlayerPrefs.GetInt("BLUECOIN");
        GameManager.instance.greenCoin = PlayerPrefs.GetInt("GREENCOIN");

        for (int i = 0; i < 7; i++)
        {
            GameManager.instance.redLevel[i] = PlayerPrefs.GetInt("REDLEVEL" + i.ToString());
            GameManager.instance.blueLevel[i] = PlayerPrefs.GetInt("BLUELEVEL" + i.ToString());
            GameManager.instance.greenLevel[i] = PlayerPrefs.GetInt("GREENLEVEL" + i.ToString());
        }
    }

    public void InitData()
    {
        GameManager.instance.curScore = 0;
        GameManager.instance.topScore = 0;

        SetData();
    }
}