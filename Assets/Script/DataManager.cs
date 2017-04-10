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
    public int tutorial = 0;    //튜토리얼 체크.

    //dataPath.
    string path = Application.persistentDataPath + "/";

    //게임 실행이 첫번째인지 판단.
    public bool FirstDecide()
    {
        first = PlayerPrefs.GetInt("FIRST");
  
        if (first == 0)
        {
            return true;
        }
        else
            return false;
    }

    //저장.
    public void SaveData()
    {
        PlayerPrefs.SetInt("WAVE", GameManager.instance.gameData.wave);
        PlayerPrefs.SetInt("TOPWAVE", GameManager.instance.gameData.topWave);
        PlayerPrefs.SetInt("COIN", GameManager.instance.gameData.coin);
        PlayerPrefs.SetInt("BALLNUM", GameManager.instance.gameData.ballNum);

        PlayerPrefs.SetInt("FIRST", first);

        if (BallManager.instance != null)
        {
            if (BallManager.instance.firstBall != null)
            {
                PlayerPrefs.SetFloat("BALLPOSX", BallManager.instance.firstBall.transform.position.x);
                PlayerPrefs.SetFloat("BALLPOSY", BallManager.instance.firstBall.transform.position.y);
            }
        }

        PlayerPrefs.SetInt("ONEMORE", GameManager.instance.oneMoreChance);
    }

    //불러오기.
    public void LoadData()
    {
        GameManager.instance.gameData.wave = PlayerPrefs.GetInt("WAVE");
        GameManager.instance.gameData.topWave = PlayerPrefs.GetInt("TOPWAVE");
        GameManager.instance.gameData.coin = PlayerPrefs.GetInt("COIN");
        GameManager.instance.gameData.ballNum = PlayerPrefs.GetInt("BALLNUM");

        GameManager.instance.oneMoreChance = PlayerPrefs.GetInt("ONEMORE");
    }

    // 정보를 저장.
    public void SaveBlodck()
    {
        //현재 씬상의 블록들을 서치.
        BlockManager.instance.FindActiveBlocks();

        //LIST내용을 저장.
        FileStream blockFile = new FileStream(path + "BlockData.txt", FileMode.Create, FileAccess.Write);

        StreamWriter blockWriter = new StreamWriter(blockFile, System.Text.Encoding.Unicode);

        foreach (GameObject _obj in BlockManager.instance.findBlock)
        {
            blockWriter.WriteLine(_obj.GetComponent<Block>().life +
                                "," + _obj.transform.position.x +
                                "," + _obj.transform.position.y +
                                "," + (int)_obj.GetComponent<Block>().blockType);
        }

        blockWriter.Close();

        blockFile.Close();
    }

    public void SaveItem()
    {
        BlockManager.instance.FindActiveItem();

        //LIST내용을 저장.
        FileStream itemFile = new FileStream(path + "ItemData.txt", FileMode.Create, FileAccess.Write);

        StreamWriter itemWriter = new StreamWriter(itemFile, System.Text.Encoding.Unicode);

        foreach (GameObject _obj in BlockManager.instance.findItem)
        {
            if (_obj != null)
            {
                itemWriter.WriteLine(_obj.transform.position.x +
                                    "," + _obj.transform.position.y);
            }
        }

        itemWriter.Close();

        itemFile.Close();
    }

    // 정보 로드.
    public string LoadFile(string dataName)
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