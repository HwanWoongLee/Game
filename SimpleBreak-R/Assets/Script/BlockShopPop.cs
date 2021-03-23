using UnityEngine;
using System.Collections;

public class BlockShopPop : MonoBehaviour
{
    [SerializeField]
    int blockNum;
    [SerializeField]
    GameObject blockCard;

    [SerializeField]
    GameObject[] exBlockEffect = new GameObject[5];
    [SerializeField]
    GameObject pop;

	public float delayTime;

    void Start()
    {
        pop.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
            {
                if (hit.collider.transform.tag.Equals("Shop"))
                {
                    BlockManager.instance.touchState = false;
                    this.gameObject.SetActive(false);
                    GameManager.instance.shopState = false;
                }
            }
        }
		if(Input.GetMouseButtonUp(0))
		{
			pop.SetActive(false);
        	exBlockEffect[0].SetActive(false);
			exBlockEffect[1].SetActive(false);
			exBlockEffect[2].SetActive(false);
			exBlockEffect[3].SetActive(false);
			exBlockEffect[4].SetActive(false);		
		}
    }


    public void FireBlockButton()
    {
        pop.SetActive(false);
        if (GameManager.instance.gameData.coin >= 50)
        {
            blockNum = BlockManager.instance.shopBlockNum;
            
            BlockManager.instance.buyState[blockNum-1] = true;

            blockCard = BlockManager.instance.target.transform.FindChild("SeletedCard(Clone)").gameObject;

            //Change Card Image
            blockCard.GetComponent<UISprite>().spriteName = "brick_red";

            //Change BlockType
            BlockManager.instance.shopBlock[blockNum - 1].blockType = Block.BlockType.fire;

            BlockManager.instance.touchState = false;
            GameManager.instance.shopState = false;
            this.gameObject.SetActive(false);

            GameManager.instance.gameData.coin -= 50;
        }
    }
    public void EraserXButton()
    {
        pop.SetActive(false);
        if (GameManager.instance.gameData.coin >= 50)
        {
            blockNum = BlockManager.instance.shopBlockNum;
            
            BlockManager.instance.buyState[blockNum-1] = true;

            blockCard = BlockManager.instance.target.transform.FindChild("SeletedCard(Clone)").gameObject;

            //Change Card Image
            blockCard.GetComponent<UISprite>().spriteName = "brick_green";

            //Change BlockType
            BlockManager.instance.shopBlock[blockNum - 1].blockType = Block.BlockType.eraserX;

            BlockManager.instance.touchState = false;
            GameManager.instance.shopState = false;
            this.gameObject.SetActive(false);

            GameManager.instance.gameData.coin -= 50;
        }
    }
    public void EraserYButton()
    {
        pop.SetActive(false);
        if (GameManager.instance.gameData.coin >= 50)
        {
            blockNum = BlockManager.instance.shopBlockNum;

            BlockManager.instance.buyState[blockNum-1] = true;

            blockCard = BlockManager.instance.target.transform.FindChild("SeletedCard(Clone)").gameObject;

            //Change Card Image
            blockCard.GetComponent<UISprite>().spriteName = "brick_blue";

            //Change BlockType
            BlockManager.instance.shopBlock[blockNum - 1].blockType = Block.BlockType.eraserY;

            BlockManager.instance.touchState = false;
            GameManager.instance.shopState = false;
            this.gameObject.SetActive(false);

            GameManager.instance.gameData.coin -= 50;
        }
    }
    public void NopButton()
    {
        pop.SetActive(false);
        if (GameManager.instance.gameData.coin >= 30)
        {
            blockNum = BlockManager.instance.shopBlockNum;
            
            BlockManager.instance.buyState[blockNum-1] = true;

            blockCard = BlockManager.instance.target.transform.FindChild("SeletedCard(Clone)").gameObject;

            //Change Card Image
            blockCard.GetComponent<UISprite>().spriteName = "brick_empty";

            //Change BlockType
            BlockManager.instance.shopBlock[blockNum - 1].blockType = Block.BlockType.nop;

            BlockManager.instance.touchState = false;
            GameManager.instance.shopState = false;
            this.gameObject.SetActive(false);

            GameManager.instance.gameData.coin -= 30;
        }
    }
    public void CoinBlockButton()
    {
        pop.SetActive(false);
        if (GameManager.instance.gameData.coin >= 30)
        {
            blockNum = BlockManager.instance.shopBlockNum;
            
            BlockManager.instance.buyState[blockNum-1] = true;

            blockCard = BlockManager.instance.target.transform.FindChild("SeletedCard(Clone)").gameObject;

            //Change Card Image
            blockCard.GetComponent<UISprite>().spriteName = "brick_yellow";

            //Change BlockType
            BlockManager.instance.shopBlock[blockNum - 1].blockType = Block.BlockType.coin;

            BlockManager.instance.touchState = false;
            GameManager.instance.shopState = false;
            this.gameObject.SetActive(false);

            GameManager.instance.gameData.coin -= 30;
        }
    }

    public void FirePress()
    {
		delayTime += Time.deltaTime;

        pop.SetActive(true);
        exBlockEffect[0].SetActive(true);
		pop.transform.localPosition = new Vector2(-1000f,-1000f);
    }
    public void EraseXPress()
    {
        pop.SetActive(true);
        exBlockEffect[2].SetActive(true);
		pop.transform.localPosition = new Vector2(0f,-1000f);
    }
    public void EraseYPress()
    {
        pop.SetActive(true);
        exBlockEffect[1].SetActive(true);
		pop.transform.localPosition = new Vector2(-500f,-1000f);
    }
    public void NopPress()
    {
        pop.SetActive(true);
        exBlockEffect[4].SetActive(true);
		pop.transform.localPosition = new Vector2(1000f,-1000f);
    }
    public void CoinPress()
    {
        pop.SetActive(true);
        exBlockEffect[3].SetActive(true);
		pop.transform.localPosition = new Vector2(500f,-1000f);
    }


}
