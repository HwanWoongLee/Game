using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour {
    public static ItemManager instance;

    int maxNum;

    public GameObject ItemPrefab;       //plus공.
    public GameObject CoinPrefab;       //coin.

    public GameObject inPotal;          //포탈.
    public GameObject outPotal;

    List<GameObject> items = new List<GameObject>();
    public List<GameObject> findItems = new List<GameObject>();

    List<GameObject> coins = new List<GameObject>();
    public List<GameObject> findCoins = new List<GameObject>();

    public int potalMade = 0;
    public int outPotalState = 0;
    public int temp;


    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        maxNum = 30;

        potalMade = PlayerPrefs.GetInt("POTALMADE");
        outPotalState = PlayerPrefs.GetInt("OUTPOTALSTATE");
        temp = PlayerPrefs.GetInt("TEMP");

        MakeItem();

        if (!DataManager.Instance.FirstDecide())
        {
            LoadItem();

            if(potalMade == 1)
            {
                inPotal.transform.position = new Vector3( PlayerPrefs.GetFloat("INXPOS"), PlayerPrefs.GetFloat("INYPOS"),0f);
                inPotal.SetActive(true);
            }
            if(temp == 1)
            {
                inPotal.transform.position = new Vector3(PlayerPrefs.GetFloat("INXPOS"), PlayerPrefs.GetFloat("INYPOS"), 0f);
                inPotal.SetActive(true);

                outPotal.transform.position = new Vector3(PlayerPrefs.GetFloat("OUTXPOS"), PlayerPrefs.GetFloat("OUTYPOS"), 0f);
                outPotal.SetActive(true);
            }
        }

        DataManager.Instance.potalProb = PlayerPrefs.GetInt("POTALPROB");
        DataManager.Instance.coinProb = PlayerPrefs.GetInt("COINPROB");
    }

    private void Update()
    {
        if(!inPotal.activeInHierarchy)
        {
            outPotal.SetActive(false);
        }
    }

    //데이터 로드.
    void LoadItem()
    {
        string data = DataManager.Instance.LoadGame("Data2.txt");
        string data2 = DataManager.Instance.LoadGame("Data3.txt");

        string[] arrData = data.Split(',', '\n');
        string[] arrData2 = data2.Split(',', '\n');

        float xpos = 0f, ypos = 0f;

        for (int i = 0; i < arrData.Length - 1; i++)
        {
            GameObject newItem = GetItem(items);

            if (i % 2 == 0)      //xpos
            {
                xpos = float.Parse(arrData[i]);
            }
            if (i % 2 == 1)     //ypos
            {
                ypos = float.Parse(arrData[i]);

                newItem.transform.position = new Vector3(xpos, ypos, 0f);

                newItem.SetActive(true);
            }
        }
        for (int i = 0; i < arrData2.Length - 1; i++)
        {
            GameObject newCoin = GetItem(coins);

            if (i % 2 == 0)      //xpos
            {
                xpos = float.Parse(arrData2[i]);
            }
            if (i % 2 == 1)     //ypos
            {
                ypos = float.Parse(arrData2[i]);

                newCoin.transform.position = new Vector3(xpos, ypos, 0f);

                newCoin.SetActive(true);
            }
        }
    }

    //만들기.
    void MakeItem()
    {
        for (int i = 0; i < maxNum; i++)
        {
            GameObject item = Instantiate(ItemPrefab,
                                            transform.position,
                                            Quaternion.identity) as GameObject;
            item.transform.parent = this.transform;                                        //자식으로 넣어 줌.
            item.SetActive(false);
            items.Add(item);
        }
        for (int i = 0; i < maxNum; i++)
        {
            GameObject coin = Instantiate(CoinPrefab,
                                            transform.position,
                                            Quaternion.identity) as GameObject;
            coin.transform.parent = this.transform;                                        //자식으로 넣어 줌.
            coin.SetActive(false);
            coins.Add(coin);
        }
    }

    public void Search()
    {
        findItems.Clear();

        foreach(GameObject _item in items)
        {
            if (_item.activeInHierarchy)
            {
                findItems.Add(_item);
            }
        }

        findCoins.Clear();

        foreach (GameObject _coin in coins)
        {
            if (_coin.activeInHierarchy)
            {
                findCoins.Add(_coin);
            }
        }
    }

    //내리기.
    public void FallItem()
    {
        foreach (GameObject _item in items)
        {
            _item.GetComponent<Item>().FallItem();
        }
        foreach (GameObject _coin in coins)
        {
            _coin.GetComponent<Item>().FallItem();
        }

        inPotal.GetComponent<Item>().FallItem();
        outPotal.GetComponent<Item>().FallItem();

        if (potalMade==1)
        {
            outPotalState = 1;
        }
    }

    //배치
    public void ArrangeItem()
    {
        int num = Random.Range(0, 7);
        int num2,num3;

        int potalNum = Random.Range(1, 101);
        int coinNum = Random.Range(1, 101);

        //블록이 생성되지 않은 위치를 num으로 지정.
        while (true)
        {
            if (BlockManager.instance.statePos[num])
            {
                num = Random.Range(0, 7);
            }
            else
                break;
        }

        //PlusBall 생성,배치.
        GameObject newItem = GetItem(items);
        newItem.transform.position = BlockManager.instance.newPos[num];
        newItem.SetActive(true);

        num2 = num;

        //potalProb 확률로.
        if (potalNum <= DataManager.Instance.potalProb+2 && potalMade==0 && !inPotal.activeInHierarchy)
        {
            //자리를 판별하고.
            while (true)
            {
                if (BlockManager.instance.statePos[num])
                {
                    num = Random.Range(0, 7);
                }
                else if(num == num2)
                {
                    num = Random.Range(0, 7);
                }
                else
                    break;
            }

            //potalIn생성.
            inPotal.transform.position = BlockManager.instance.newPos[num];
            inPotal.SetActive(true);
            potalMade = 1;
        }
        if (outPotalState == 1)
        {
            num = Random.Range(0, 7);
            //자리를 판별하고.
            while (true)
            {
                if (BlockManager.instance.statePos[num])
                {
                    num = Random.Range(0, 7);
                }
                else if (num == num2)
                {
                    num = Random.Range(0, 7);
                }
                else
                    break;
            }

            //potalOut생성.
            outPotal.transform.position = BlockManager.instance.newPos[num];
            outPotal.SetActive(true);
            outPotalState = 0;
            potalMade = 0;
        }

        num3 = num;

        //coinProb 확률로.
        if (coinNum <= (DataManager.Instance.coinProb +5))
        {
            //자리를 판별하고.
            while (true)
            {
                if (BlockManager.instance.statePos[num])
                {
                    num = Random.Range(0, 7);
                }
                else if (num == num2)
                {
                    num = Random.Range(0, 7);
                }
                else if (num == num3)
                {
                    num = Random.Range(0, 7);
                }
                else
                    break;
            }

            //coin생성.
            GameObject newCoin = GetItem(coins);
            newCoin.transform.position = BlockManager.instance.newPos[num];
            newCoin.SetActive(true);
        }
  

    }

    //List에서 불러온다.
    GameObject GetItem(List<GameObject> Obj)
    {
        foreach (GameObject _obj in Obj)
        {
            if (!_obj.activeInHierarchy)
                return _obj;
        }
        return null;
    }
}