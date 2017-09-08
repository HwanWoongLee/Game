using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[Serializable]
public class StatData
{
    public string type;
    public int level;
    public int damage;
    public float moveSpeed;
    public float fireSpeed;
}

[Serializable]
public class StoreData
{
    public int level;
    public int damagePrice;
    public int moveSpeedPrice;
    public int fireSpeedPrice;
}


public class JsonData : MonoBehaviour
{
    string[] dataPath = new string[7];
    TextAsset[] androidPath = new TextAsset[7];

    string storeDataPath;
    TextAsset andStoreDataPath;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            androidPath[0] = Resources.Load("normalData") as TextAsset;
            androidPath[1] = Resources.Load("bigData") as TextAsset;
            androidPath[2] = Resources.Load("laserData") as TextAsset;
            androidPath[3] = Resources.Load("bounceData") as TextAsset;
            androidPath[4] = Resources.Load("guidedData") as TextAsset;
            androidPath[5] = Resources.Load("swordData") as TextAsset;
            androidPath[6] = Resources.Load("explosionData") as TextAsset;

            andStoreDataPath = Resources.Load("StoreData") as TextAsset;
        }
        else if(Application.platform == RuntimePlatform.IPhonePlayer)
        {

        }
        else
        {
            dataPath[0] = Application.dataPath + "/Resources/normalData.json";
            dataPath[1] = Application.dataPath + "/Resources/bigData.json";
            dataPath[2] = Application.dataPath + "/Resources/laserData.json";
            dataPath[3] = Application.dataPath + "/Resources/bounceData.json";
            dataPath[4] = Application.dataPath + "/Resources/guidedData.json";
            dataPath[5] = Application.dataPath + "/Resources/swordData.json";
            dataPath[6] = Application.dataPath + "/Resources/explosionData.json";

            storeDataPath = Application.dataPath + "/Resources/StoreData.json";
        }
    }

    //초기 jsonData file setting
    public void SaveData(int pathNum, string name)
    {
        StatData[] statData = new StatData[5];

        for (int i = 0; i < 5; i++)
        {

            statData[i] = new StatData();
            statData[i].type = name;
            statData[i].level = i + 1;
            statData[i].damage = i + 1;
            statData[i].moveSpeed = i + 1;
            statData[i].fireSpeed = 5 - i;

        }

        string toJson = JsonHelper.ToJson(statData, prettyPrint: true);

        File.WriteAllText(dataPath[pathNum], toJson);
    }

    //type,level,필요 data별로 return
    public StatData LoadData(int _type, int level)
    {
        //json 문자열
        string json;

        StatData[] statData = null;

        if (Application.platform == RuntimePlatform.Android)
        {
            //path에 있는 파일을 json문자열에 저장
            json = androidPath[_type].ToString();

            //문자열을 json 형식으로 전환
            statData = JsonHelper.FromJson<StatData>(json);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            json = androidPath[_type].ToString();
            statData = JsonHelper.FromJson<StatData>(json);
        }
        else
        {
            json = File.ReadAllText(dataPath[_type]);

            statData = JsonHelper.FromJson<StatData>(json);
        }

        return statData[level];   
    }

    public StoreData LoadStoreData(int level)
    {
        string json;

        StoreData[] storeData = null;

        if (Application.platform == RuntimePlatform.Android)
        {
            //path에 있는 파일을 json문자열에 저장
            json = andStoreDataPath.ToString();
            
            //문자열을 json 형식으로 전환
            storeData = JsonHelper.FromJson<StoreData>(json);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            //path에 있는 파일을 json문자열에 저장
            json = andStoreDataPath.ToString();

            //문자열을 json 형식으로 전환
            storeData = JsonHelper.FromJson<StoreData>(json);
        }
        else
        {
            json = File.ReadAllText(storeDataPath);

            storeData = JsonHelper.FromJson<StoreData>(json);
            
        }

        return storeData[level];
    }
}