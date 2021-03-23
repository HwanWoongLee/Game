using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockManager : MonoBehaviour {
    public static BlockManager instance;

    int maxNum = 50;        //블록최대개수.
    int colBlockNum;        //블록생성개수.
    int posx;               //블록 간격.

    public GameObject blockPrefab;      //블록.

    public List<GameObject> findBlocks = new List<GameObject>();
    List<GameObject> blocks = new List<GameObject>();

    public Vector3[] newPos;      //생성위치.
    public bool[] statePos;

    private int blockType;

    void Awake()
    {
        if (instance == null)
            instance = this;

        newPos = new Vector3[7];      //생성위치.
        statePos = new bool[7];

        //블록 생성.
        MakeBlock();        

        //블록 간격.
        posx = 2;           
        
        //생성위치 지정.
        for (int i = 0; i < newPos.Length; i++)
        {
            newPos[i] = new Vector3(-3f + ((i) * 1f), 4f, 0f);
        }
    }

    void Start() {
        if (DataManager.Instance.FirstDecide())
            FirstStartSet();
        else
            LoadStartSet();

        DataManager.Instance.fireProb = PlayerPrefs.GetInt("FIREPROB");
        DataManager.Instance.eraserProb = PlayerPrefs.GetInt("ERASERPROB");
    }

    //게임 처음 시작 블록 세팅.
    void FirstStartSet()
    {
        ArrangeBlock();     //블록 배치.

        ItemManager.instance.ArrangeItem();     //아이템배치.

        GameManager.instance.curGameState = GameManager.GameState.moveblock;

    }

    //게임 불러오기.
    void LoadStartSet()
    {
        //블록데이터 (life,x,y)를 받아서 배치.
        string data = DataManager.Instance.LoadGame("Data.txt");
        string []arrData = data.Split(',','\n');

        float xpos = 0f, ypos = 0f;

        for (int i = 0; i < arrData.Length -1; i++)
        {
            //비활성화인 블록하나를 가져옴.
            GameObject newblock = GetBlock();

            if (i % 4 == 0)     //life
            {
                newblock.GetComponent<Block>().blockLife = int.Parse(arrData[i]);
            }
            else if (i % 4 == 1)    //xpos
            {
                xpos = float.Parse(arrData[i]);
            }
            else if (i % 4 == 2)    //ypos
            {
                ypos = float.Parse(arrData[i]);
                //배치.
                newblock.transform.position = new Vector3(xpos, ypos, 0f);
            }
            else if(i % 4 == 3)     //type
            {
                newblock.GetComponent<Block>().thisBlockType = (Block.BlockType)int.Parse(arrData[i]);
               
                //활성화.
                newblock.SetActive(true);
            }
        }
    }

    //블록만들기.
    void MakeBlock()
    {
        for (int i = 0; i < maxNum; i++)
        {
            GameObject block = Instantiate(blockPrefab, 
                                            transform.position, 
                                            Quaternion.identity) as GameObject;
            block.GetComponent<Block>().blockLife = GameManager.instance.curWave;           //현재 단계가 블록 생명.
            block.transform.parent = this.transform;                                        //자식으로 넣어 줌.
            block.SetActive(false);
            blocks.Add(block);
        }
    }

    //블록내리기.
    public void FallBlock()
    {
        foreach (GameObject _block in blocks)
        {
            _block.GetComponent<Block>().FallBlock();
        }
    }

    //블록배치
    public void ArrangeBlock()
    {
        int num = BlockNumbering();     //개수 받아옴.

        int ranNum;
        int[] arrayNum = new int[7];    //중복검사 변수.

        DataManager.Instance.fireProb = PlayerPrefs.GetInt("FIREPROB");
        DataManager.Instance.eraserProb = PlayerPrefs.GetInt("ERASERPROB");

        //생성위치의 상태를 초기화.
        for (int k = 0; k < statePos.Length; k++)
        {
            statePos[k] = false;
        }

        //중복검사 변수 초기화.
        for (int i=0;i<7; i++)
        {
            arrayNum[i] = -1;
        }

        for (int i = 0; i < num; i++)
        {
            ranNum = Random.Range(0, 7);
          
            for (int j = 0; j < 7; j++)
            {
                if (ranNum == arrayNum[j])      //중복검사.
                {
                    ranNum = Random.Range(0, 7);
                }
            }

            //블록 배치.
            GameObject newBlock = GetBlock();
            newBlock.transform.position = newPos[ranNum];

            int randNum = Random.Range(1, 101);
            int randNum2 = Random.Range(1, 101);

            if (randNum <= DataManager.Instance.fireProb+2)   //fire블록 확률.
            {
                blockType = 1;
            }
            else if (randNum2 <= DataManager.Instance.eraserProb +2)   //지우개블록 확률.
            {
                blockType = 2;
            }
            else
            {
                blockType = 0;          //기본노말.
            }

            newBlock.GetComponent<Block>().thisBlockType = (Block.BlockType)blockType;
            newBlock.SetActive(true);

            //생성위치 상태 체크.
            statePos[ranNum] = true;
            arrayNum[i] = ranNum;

        }

    }

    //List에서 블록을 불러온다.
    GameObject GetBlock()
    {
        foreach (GameObject _block in blocks)
        {
            if (!_block.activeInHierarchy)
                return _block;
        }
        return null;
    }

    //블록상태초기화.
    public void InitBlock()
    {
        foreach (GameObject _block in blocks)
        {
            //비활성화 된 블록.
            if (!_block.activeInHierarchy)
            {
                _block.GetComponent<Block>().blockLife = GameManager.instance.curWave+1;
            }
        }
    }

    //블록개수지정(단계별로 랜덤값)
    int BlockNumbering()
    {
        if (GameManager.instance.curWave < 5)
        {
            colBlockNum = 1;
        }
        else if (GameManager.instance.curWave >= 5 && GameManager.instance.curWave < 10)
        {
            colBlockNum = Random.Range(1, 3);
        }
        else if (GameManager.instance.curWave >= 10 && GameManager.instance.curWave < 15)
        {
            colBlockNum = Random.Range(2, 4);
        }
        else if (GameManager.instance.curWave >= 15 && GameManager.instance.curWave < 25)
        {
            colBlockNum = Random.Range(3, 5);
        }
        else if (GameManager.instance.curWave >= 25 && GameManager.instance.curWave < 50)
        {
            colBlockNum = Random.Range(3, 6);
        }
        else if (GameManager.instance.curWave >= 50)
        {
            colBlockNum = Random.Range(4, 6);
        }

        return colBlockNum;
    }

    //활성화 되있는 블록찾기.
    public void Search()
    {
        //list지우고.
        findBlocks.Clear();

        //활성화 되있는 블록을 넣어줌.
        foreach (GameObject _block in blocks)
        {
            if(_block.activeInHierarchy)
            {
                findBlocks.Add(_block);
            }
        }
    }
}