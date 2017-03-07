using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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

    public int first;          //첫번째 시작 검사.
    public int term =0;        //강제종료 검사.

    //dataPath.
    string path = Application.persistentDataPath + "/";

    public int fireProb = 0;       //fire블록 활률.
    public int eraserProb = 0;     //eraser확률.
    public int potalProb = 0;
    public int coinProb = 0;


    //게임 실행이 첫번째인지 판단.
    public bool FirstDecide()
    {
        first = PlayerPrefs.GetInt("FIRST");

        if (first == 0)
        {
            first++;
            return true;
        }
        else
            return false;
    }

    public void InitData()
    {
        PlayerPrefs.SetInt("BALLNUM", 1);
        PlayerPrefs.SetInt("CURWAVE", 1);
        PlayerPrefs.SetInt("FIRST", 0);

        //PlayerPrefs.SetInt("TOPWAVE", 1);
        //PlayerPrefs.SetInt("COIN", 0);
    }

    //저장.
    public void SaveData()
    {
        if (GameManager.instance == null || ItemManager.instance == null || BallManager.instance == null)
            return;

        PlayerPrefs.SetInt("BALLNUM", GameManager.instance.ballNum);
        PlayerPrefs.SetInt("CURWAVE", GameManager.instance.curWave);
        PlayerPrefs.SetInt("TOPWAVE", GameManager.instance.topWave);
        PlayerPrefs.SetInt("COIN", GameManager.instance.coin);

        PlayerPrefs.SetInt("FIRST", first);
        PlayerPrefs.SetInt("TERM", term);

        PlayerPrefs.SetInt("ONEMORE", GameManager.instance.oneMoreCheck);
        PlayerPrefs.SetInt("GAMEOVER", GameManager.instance.gameOverCheck);

        //potal save.
        PlayerPrefs.SetInt("POTALMADE", ItemManager.instance.potalMade);
        PlayerPrefs.SetInt("OUTPOTALSTATE", ItemManager.instance.outPotalState);

        PlayerPrefs.SetFloat("INXPOS", ItemManager.instance.inPotal.transform.position.x);
        PlayerPrefs.SetFloat("INYPOS", ItemManager.instance.inPotal.transform.position.y);
        PlayerPrefs.SetFloat("OUTXPOS", ItemManager.instance.outPotal.transform.position.x);
        PlayerPrefs.SetFloat("OUTYPOS", ItemManager.instance.outPotal.transform.position.y);

        if (ItemManager.instance.outPotal.activeInHierarchy)
        {
            PlayerPrefs.SetInt("TEMP", 1);
        }
        else
        {
            PlayerPrefs.SetInt("TEMP", 0);
        }

        PlayerPrefs.SetFloat("BALLX", BallManager.instance.setPos.x);
        PlayerPrefs.SetFloat("BALLY", BallManager.instance.setPos.y);

    }

    //불러오기.
    public void LoadData()
    {
        GameManager.instance.ballNum = PlayerPrefs.GetInt("BALLNUM");
        GameManager.instance.curWave = PlayerPrefs.GetInt("CURWAVE");
        GameManager.instance.topWave = PlayerPrefs.GetInt("TOPWAVE");
        GameManager.instance.coin = PlayerPrefs.GetInt("COIN");

        GameManager.instance.oneMoreCheck = PlayerPrefs.GetInt("ONEMORE");
        GameManager.instance.gameOverCheck = PlayerPrefs.GetInt("GAMEOVER");
    }

    // 정보를 저장.
    public void SaveGame()
    {
        //현재 씬상의 블록들을 서치.
        BlockManager.instance.Search();
        ItemManager.instance.Search();

        //LIST내용을 저장.
        FileStream blockFile = new FileStream(path + "Data.txt", FileMode.Create, FileAccess.Write);
        FileStream itemFile = new FileStream(path + "Data2.txt", FileMode.Create, FileAccess.Write);
        FileStream coinFile = new FileStream(path + "Data3.txt", FileMode.Create, FileAccess.Write);

        StreamWriter blockWriter = new StreamWriter(blockFile, System.Text.Encoding.Unicode);
        StreamWriter itemWriter = new StreamWriter(itemFile, System.Text.Encoding.Unicode);
        StreamWriter coinWriter = new StreamWriter(coinFile, System.Text.Encoding.Unicode);

        foreach (GameObject _obj in BlockManager.instance.findBlocks)
        {
            blockWriter.WriteLine(_obj.GetComponent<Block>().blockLife +
                                "," + _obj.transform.position.x +
                                "," + _obj.transform.position.y +
                                "," + (int)_obj.GetComponent<Block>().thisBlockType);
        }

        foreach (GameObject _obj in ItemManager.instance.findItems)
        {
            itemWriter.WriteLine(_obj.transform.position.x +
                                "," + +_obj.transform.position.y);
        }

        foreach (GameObject _obj in ItemManager.instance.findCoins)
        {
            coinWriter.WriteLine(_obj.transform.position.x +
                                "," + +_obj.transform.position.y);
        }
        blockWriter.Close();
        itemWriter.Close();
        coinWriter.Close();

        blockFile.Close();
        itemFile.Close();
        coinFile.Close();
    }

    // 정보 로드.
    public string LoadGame(string dataName)
    {
        FileStream File = new FileStream(path + dataName, FileMode.Open, FileAccess.Read);

        StreamReader sr = new StreamReader(File);

        string str = null;

        str = sr.ReadToEnd();

        sr.Close();
        File.Close();

        return str;
    }

}
