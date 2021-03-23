using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance;

    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private GameObject plusPrefab;
    [SerializeField]
    private GameObject blockShopPop;
    public GameObject target = null;           //shop slot card

    [SerializeField]
    private List<GameObject> slotBlock = new List<GameObject>();
    public List<GameObject> allBlock = new List<GameObject>();
    [SerializeField]
    private List<GameObject> allItem = new List<GameObject>();
    public List<GameObject> findBlock = new List<GameObject>();
    public List<GameObject> findItem = new List<GameObject>();
    public GameObject warpIn,warpOut, dLine;

    public Block[] shopBlock = new Block[7];

    public bool[] buyState = new bool[7];

    public int shopBlockNum;

    public bool touchState;
    public bool warpState;
    
    GameObject newblock, newitem;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        slotBlock.Clear();

        for (int i = 0; i < shopBlock.Length; i++)
        {
            shopBlock[i] = null;
        }

        if (!DataManager.Instance.FirstDecide())
        {
            LoadBlock();
            LoadItem();
        }
    }
    void LoadBlock()
    {
        //블록데이터 (life,x,y)를 받아서 배치.
        string data = DataManager.Instance.LoadFile("BlockData.txt");

        if (data == null)
        {
            return;
        }
        string[] arrData = data.Split(',', '\n');

        float xpos = 0f, ypos = 0f;

        for (int i = 0; i < arrData.Length - 1; i++)
        {
            if (i % 4 == 0)     //life
            {
                newblock = Instantiate(blockPrefab);
                newblock.transform.parent = this.transform;

                //비활성화
                newblock.SetActive(false);

                //리스트Add
                allBlock.Add(newblock);

                newblock.GetComponent<Block>().life = int.Parse(arrData[i]);
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
            else if (i % 4 == 3)     //type
            {
                newblock.GetComponent<Block>().blockType = (Block.BlockType)int.Parse(arrData[i]);

                //활성화.
                newblock.SetActive(true);
            }
        }
    }

    void LoadItem()
    {
        string data = DataManager.Instance.LoadFile("ItemData.txt");
        if (data == null)
        {
            return;
        }

        string[] arrData = data.Split(',', '\n');

        float xpos = 0f, ypos = 0f;

        for (int i = 0; i < arrData.Length - 1; i++)
        {
            if (i % 2 == 0)    //xpos
            {
                newitem = Instantiate(plusPrefab);
                newitem.transform.parent = this.transform;

                //비활성화
                newitem.SetActive(false);

                //리스트Add
                allItem.Add(newitem);

                xpos = float.Parse(arrData[i]);
            }
            else if (i % 2 == 1)    //ypos
            {
                ypos = float.Parse(arrData[i]);
                //배치.
                newitem.transform.position = new Vector3(xpos, ypos, 0f);
                //활성화.
                newitem.SetActive(true);
            }
        }
    }


    //블록생성
    public void MakeBlocks()
    {
        GameObject newblock = Instantiate(blockPrefab);
        newblock.transform.parent = this.transform;
        newblock.transform.position = Vector3.zero;
        newblock.GetComponent<Block>().blockType = Block.BlockType.normal;

        //비활성화
        newblock.SetActive(false);

        //리스트Add
        slotBlock.Add(newblock);
        allBlock.Add(newblock);
    }

    //블록배치
    public void ArrangeBlocks(int num, int i)
    {
        slotBlock[num].transform.position = new Vector3(i - 3, -3.5f, -1f);
        slotBlock[num].GetComponent<Block>().life = GameManager.instance.gameData.wave;
        shopBlock[i] = slotBlock[num].GetComponent<Block>();
    }

    //공 추가 아이템 배치
    public void ArrangePlusItem(int i)
    {
        GameObject newItem = Instantiate(plusPrefab);
        newItem.transform.parent = this.transform;
        newItem.transform.position = new Vector3(i - 3, -3.5f, -1f);
        allItem.Add(newItem);
    }

    //워프 배치
    public void ArrangeWarpInItem(int i)
    {
        warpIn.SetActive(true);
        warpIn.transform.parent = this.transform;
        warpIn.transform.position = new Vector3(i - 3, -3.5f, -1f);
        allItem.Add(warpIn);
    }
    public void ArrangeWarpOutItem(int i)
    {
        warpOut.SetActive(true);
        warpOut.transform.parent = this.transform;
        warpOut.transform.position = new Vector3(i - 3, -3.5f, -1f);
        allItem.Add(warpOut);
    }
    public void OffWarp()
    {
        warpIn.GetComponent<Item>().moveStart = false;
        warpOut.GetComponent<Item>().moveStart = false;
        
        warpIn.SetActive(false);
        warpOut.SetActive(false);
        warpState = false;
    }

    //배치 된 블록 활성화
    public void ActiveBlocks()
    {
        foreach (GameObject block in slotBlock)
        {
            block.SetActive(true);
        }
        slotBlock.Clear();
    }

    //블록 올리기
    public void MoveBlocks()
    {
        for (int i = allBlock.Count - 1; i >= 0; i--)
        {
            if (allBlock[i] != null)
            {
                if(allBlock[i].activeInHierarchy)
                    allBlock[i].GetComponent<Block>().moveStart = true;
            }
            else
            {
                allBlock.Remove(allBlock[i]);
            }
        }
        for (int i = allItem.Count - 1; i >= 0; i--)
        {
            if (allItem[i] != null)
            {
                if(allItem[i].activeInHierarchy)
                    allItem[i].GetComponent<Item>().moveStart = true;
            }
            else
            {
                allItem.Remove(allItem[i]);
            }
        }

        for (int i = 0; i < buyState.Length; ++i)
        {
            buyState[i] = false;
        }
    }

    //touch check in slot
    public void TouchSlotCheck()
    {
        if (Input.GetMouseButtonDown(0) && !touchState)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
            {
                target = hit.collider.gameObject;

                if (target.transform.FindChild("SeletedCard(Clone)") != null)
                {
                    switch (target.name)
                    {
                        case "1":
                            shopBlockNum = 1;
                            break;
                        case "2":
                            shopBlockNum = 2;
                            break;
                        case "3":
                            shopBlockNum = 3;
                            break;
                        case "4":
                            shopBlockNum = 4;
                            break;
                        case "5":
                            shopBlockNum = 5;
                            break;
                        case "6":
                            shopBlockNum = 6;
                            break;
                        case "7":
                            shopBlockNum = 7;
                            break;
                    }

                    if (!buyState[shopBlockNum - 1])
                    {
                        SoundManager.instance.PlayEffectSound(12);

                        blockShopPop.GetComponent<TweenAlpha>().ResetToBeginning();
                        blockShopPop.GetComponent<TweenAlpha>().Play();
                        blockShopPop.SetActive(true);
                        GameManager.instance.shopState = true;
                        touchState = true;
                    }

                }
            }
        }
    }

    //find active block
    public void FindActiveBlocks()
    {
        //list지우고.
        findBlock.Clear();

        //활성화 되있는 블록을 넣어줌.
        foreach (GameObject _block in allBlock)
        {
            if (_block != null)
            {
                if (_block.activeInHierarchy)
                {
                    findBlock.Add(_block);
                }
            }
        }
    }

    public void FindActiveItem()
    {
        findItem.Clear();

        foreach (GameObject _item in allItem)
        {
            if (_item != null)
            {
                if (_item.activeInHierarchy)
                {
                    findItem.Add(_item);
                }
            }
        }
    }
   
    //광고시청후 블록 삭제
    public void OneMoreAdsAfter()
    {
        FindActiveBlocks();

        foreach (GameObject _block in allBlock)
        {
            if (_block.transform.position.y >= 2.5f)
            {
                Destroy(_block);
            }
        }
    }
}
