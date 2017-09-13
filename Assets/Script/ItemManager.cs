using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : ObjectManager  
{
    public static ItemManager instance;
    
    [SerializeField]
    private float rendDeleyTime = 1.5f;
    private float curTime = 0f;
    private float x, y;

    [SerializeField]
    private float maxRange, minRange;

    public List<GameObject> boxItems = new List<GameObject>();

    public GameObject starItem;
    public GameObject fever;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void Start()
    {
        MakeItems();
    }

    //배열마다 아이템 개수에 맞춰 생성
    private void MakeItems()
    {
        for (int i = 0; i < this.makeObj.Length; i++)
        {
            this.maxNum = 20;

            if (i != 0)
            {
                this.maxNum = 2;
            }
            MakeObjs(this.makeObj[i]);
        }
    }

    //Item Render
    public void RendItem()
    {
        curTime += Time.deltaTime;

        if (curTime >= rendDeleyTime)
        {
            GameObject newItem = GetItem();

            if (newItem)
            {
                SetPos();

                newItem.transform.position = new Vector3(x, y, 0);
                newItem.SetActive(true);

                curTime = 0f;
            }
        }
    }

    //이미 생성한 아이템중 랜덤하게 아이템을 가져옴
    private GameObject GetItem()
    {
        int num = Random.Range(0, objList.Count);

        //이미 활성화 되있다면 다시 랜덤.
        while (objList[num].activeInHierarchy)
        {
            num = Random.Range(0, objList.Count);
        }

        return objList[num];
    }

    //x,y 위치지정
    private void SetPos()
    {
        if(maxRange < minRange)
            return;
        
        x = Random.Range(player.transform.position.x - maxRange, player.transform.position.x + maxRange);
        y = Random.Range(player.transform.position.y - maxRange, player.transform.position.y + maxRange);


        while (Mathf.Abs(x - player.transform.position.x) <= minRange &&
                Mathf.Abs(y - player.transform.position.y) <= minRange)
        {
            x = Random.Range(player.transform.position.x - maxRange, player.transform.position.x + maxRange);
            y = Random.Range(player.transform.position.y - maxRange, player.transform.position.y + maxRange);
        }
    }

    public void RendStarItem(float _time)
    {
        Invoke("ActiveStar",_time);
    }
    public void RendFever()
    {
        Invoke("ActiveFever", 1.6f);
    }

    public void ActiveStar()
    {
        int randNum = Random.Range(0, 2);

        if (randNum == 0)
        {
            SetPos();

            if (starItem)
                starItem.transform.position = new Vector3(x, y, 0);
            starItem.SetActive(true);
        }
        else
        {
            return;
        }
    }

    void ActiveFever()
    {
        fever.GetComponent<Fever>().fever = true;
        SoundManager.instance.PlayEffectSound(17);
    }
}
