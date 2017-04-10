using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardDeck : MonoBehaviour
{
    [SerializeField]
    private Camera uiCam;

    public GameObject BlockCard;
    public List<GameObject> cardList;

    private bool mouseState = false;            //마우스 상태
    private bool selectState = false;

    private GameObject target = null;           //터치 타겟

    [SerializeField]
    private UISprite selectCardImage;           //선택된 카드 이미지

    //[HideInInspector]
    public int[] arrangeBlock = new int[7];     //0 = null, 1 = 배치

    public bool warpIn;
    public bool warpOut;

    private void Start()
    {
        for (int i = 0; i < arrangeBlock.Length; i++)
        {
            arrangeBlock[i] = 0;
        }
        warpIn = false;
        warpOut = false;

    }


    //블록카드 선택하기
    public void BlockCardSelect()
    {
        #region 클릭체크, 카드선택
        //클릭다운
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = uiCam.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
            {
                target = hit.collider.gameObject;
                //타겟이 카드이면
                if (target.tag.Equals("Card"))
                {
                    //마우스 트루
                    mouseState = true;
                    selectState = true;

                     SoundManager.instance.PlayEffectSound(11);
                }
                else if (target.tag.Equals("Slot"))
                {
                    mouseState = false;
                    selectState = false;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //마우스를 때면 false
            mouseState = false;

            Ray ray = uiCam.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction * 10, out hit) && selectState)
            {
                target = hit.collider.gameObject;
                //타겟이 슬롯
                if (target.tag.Equals("Slot"))
                {
                    SoundManager.instance.PlayEffectSound(10);

                    selectState = false;

                    if (arrangeBlock[int.Parse(target.name) - 1] == 0)
                    {
                        //해당 슬롯에 블록생성.
                        UISprite select = Instantiate(selectCardImage);
                        select.transform.parent = target.transform;
                        select.transform.localPosition = Vector2.zero;
                        select.transform.localScale = new Vector3(1f, 1f, 1f);

                        //드래그이미지 해제
                        selectCardImage.gameObject.SetActive(false);

                        //슬롯번호적용
                        CheckSlot(target.name);

                        //다 올렸는지 검사
                        CheckCard();
                    }
                    else if (arrangeBlock[int.Parse(target.name) - 1] == 1)  //중복일시.
                    {
                        //드래그이미지 해제
                        selectCardImage.gameObject.SetActive(false);
                        SelectOffTarget();
                    }
                }
            }
        }
        #endregion

        #region 타겟효과
        if (selectState)
        {
            if (mouseState)
            {
                SelectOnTarget();
            }
            else
            {
                SelectOffTarget();
            }
        }
        #endregion
    }
    void SelectOnTarget()
    {
        Destroy(target);

        selectCardImage.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                                    Input.mousePosition.y,
                                                                    -Camera.main.transform.position.z));
        selectCardImage.gameObject.SetActive(true);
        this.GetComponent<UIGrid>().enabled = true;
    }
    void SelectOffTarget()
    {
        GameObject returnCard = Instantiate(BlockCard);
        returnCard.transform.parent = this.transform;
        returnCard.transform.localScale = new Vector3(1f, 1f, 1f);
        returnCard.transform.localPosition = new Vector3(0f, 0f, 0f);

        selectCardImage.gameObject.SetActive(false);
        this.GetComponent<UIGrid>().enabled = true;

        selectState = false;
    }

    bool sibal;
    //블록카드검사,블록생성배치.
    void CheckCard()
    {
        int num = 0;
        int num2 = 0;

        bool plusItem = false;
        sibal = false;

        if (this.gameObject.transform.FindChild("BlockCard(Clone)") == null)
        {
            for (int i = 0; i < arrangeBlock.Length; ++i)
            {
                //1인 슬롯에 블록 생성.
                if (arrangeBlock[i] == 1)
                {
                    //Block Make
                    BlockManager.instance.MakeBlocks();
                    BlockManager.instance.ArrangeBlocks(num, i);
                    num++;
                }
                else if (arrangeBlock[i] == 0 && !plusItem)
                {
                    num2 = i;

                    int randNum = Random.Range(0, 2);
                    if (randNum == 1)
                    {
                        arrangeBlock[i] = 1;
                        BlockManager.instance.ArrangePlusItem(num2);
                        plusItem = true;
                    }
                }
                else if (arrangeBlock[i] == 0 && !warpIn)
                {
                    int randNum = Random.Range(0, 30);

                    if (randNum == 1)
                    {
                        BlockManager.instance.ArrangeWarpInItem(i);
                        warpIn = true;
                    }
                }
                else if (arrangeBlock[i] == 0 && warpOut)
                {
                    int randNum = Random.Range(0, 2);

                    if (randNum == 1)
                    {
                        BlockManager.instance.ArrangeWarpOutItem(i);
                        sibal = true;
                    }
                }
            }

            if (!sibal && warpOut)
            {
                for (int i = 0; i < arrangeBlock.Length; i++)
                {
                    if (arrangeBlock[i] == 0)
                    {
                        BlockManager.instance.ArrangeWarpOutItem(i);
                        sibal = true;
                    }
                }
            }

            if (!plusItem)
            {
                arrangeBlock[num2] = 1;
                BlockManager.instance.ArrangePlusItem(num2);
            }

            if (warpIn)
                warpOut = true;

            //생성후 배열 초기화
            for (int i = 0; i < arrangeBlock.Length; i++)
            {
                arrangeBlock[i] = 0;
            }

            //ReadyState
            GameManager.instance.curGameState = GameState.Ready;
        }
    }

    //슬롯적용
    void CheckSlot(string name)
    {
        int num = int.Parse(name);
        arrangeBlock[num - 1] = 1;
    }

    //블록카드 만들기
    public void BlockCardMake()
    {
        int num = GetBlockNum();

        //리스트 초기화.
        cardList.Clear();

        for (int i = 0; i < num; i++)
        {
            GameObject newCard = Instantiate(BlockCard);
            newCard.transform.parent = this.transform;
            newCard.transform.localScale = new Vector3(1f, 1f, 1f);
            newCard.transform.localPosition = new Vector3(0f, 0f, 0f);
            newCard.SetActive(false);

            //현재 카드들을 리스트에 넣어줌.
            cardList.Add(newCard);
        }

        BlockCardAppear();

        GameManager.instance.curGameState = GameState.SelectBlock;
    }

    //블록카드 보여주기
    void BlockCardAppear()
    {
        foreach (GameObject card in cardList)
        {
            card.transform.localPosition = new Vector3(0f, 0f, 0f);
            card.SetActive(true);
        }

        this.GetComponent<UIGrid>().enabled = true;
    }

    //블록카드 개수 받아오기
    private int GetBlockNum()
    {
        int randNum = 0;

        if (GameManager.instance.gameData.wave <= 19)
        {
            randNum = Random.Range(1, 4);
        }
        else if (GameManager.instance.gameData.wave <= 79)
        {
            randNum = Random.Range(1, 6);
        }
        else if (GameManager.instance.gameData.wave <= 99)
        {
            randNum = Random.Range(4, 6);
        }
        else if (GameManager.instance.gameData.wave > 99)
        {
            randNum = 5;
        }

        return randNum;
    }
}
