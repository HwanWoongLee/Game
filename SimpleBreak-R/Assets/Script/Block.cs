using UnityEngine;
using System.Collections;

public class Block : Obj
{
    public enum BlockType
    {
        normal,
        fire,
        eraserX,
        eraserY,
        nop,
        coin,
        none

    }

    public BlockType blockType;

    [SerializeField]
    private SpriteRenderer image;
    [SerializeField]
    private GameObject[] bEffect;
    [SerializeField]
    private GameObject[] eEffect;

    [SerializeField]
    private UILabel lifeLabel;
    public int life;

    [SerializeField]
    private Sprite[] blockImage;

    public GameObject getCoin1, getCoin2;

    private void Update()
    {
        if (GameManager.instance == null)
            GameManager.instance = GameObject.Find("@GameManager").GetComponent<GameManager>();

        if (moveStart)
        {
            MoveBlock(this.transform);
        }

        if (lifeLabel != null && GameManager.instance.curGameState != GameState.GameOver)
        {
            lifeLabel.text = life.ToString();
        }
        else
        {
            lifeLabel.text = " ";
        }

        ChangeBlockType();

        if (GameManager.instance.curGameState == GameState.Fire)
        {
            if (this.blockType == BlockType.nop)
            {
                StartCoroutine(disappearBlock());
            }
        }
    }

    private IEnumerator disappearBlock()
    {
        yield return new WaitForSeconds(.5f);

        this.blockType = BlockType.none;
        RemoveBlock(this.blockType);

        GameObject newEffect = Instantiate(bEffect[4]);
        newEffect.transform.position = this.transform.position;
    }

    private void ChangeBlockType()
    {
        switch (this.blockType)
        {
            case BlockType.fire:
                this.image.sprite = blockImage[0];
                break;
            case BlockType.eraserX:
                this.image.sprite = blockImage[2];
                break;
            case BlockType.eraserY:
                this.image.sprite = blockImage[1];
                break;
            case BlockType.coin:
                this.image.sprite = blockImage[4];
                break;
            case BlockType.nop:
                this.image.sprite = blockImage[5];
                break;
        }
    }

    private void RemoveBlock(BlockType _type)       //addcoin, blockremove
    {
        BlockManager.instance.FindActiveBlocks();

        GameObject coinEffect;
        GameObject effects;

        switch (_type)
        {
            case BlockType.normal:
                SoundManager.instance.PlayEffectSound(8);

                GameManager.instance.AddCoin(1);

                coinEffect = Instantiate(getCoin1);
                coinEffect.GetComponent<TweenPosition>().from = this.transform.position;
                coinEffect.GetComponent<TweenPosition>().to = this.transform.position + (Vector3.up * 2);

                effects = Instantiate(eEffect[4]);
                effects.transform.position = this.transform.position;
                break;
            case BlockType.fire:
                SoundManager.instance.PlayEffectSound(3);

                GameManager.instance.AddCoin(1);
                GameManager.instance.gameData.coin -= 10;

                coinEffect = Instantiate(getCoin1);
                coinEffect.GetComponent<TweenPosition>().from = this.transform.position;
                coinEffect.GetComponent<TweenPosition>().to = this.transform.position + (Vector3.up * 2);

                for (int i = BlockManager.instance.findBlock.Count - 1; i >= 0; i--)
                {
                    float x = this.transform.position.x - BlockManager.instance.findBlock[i].transform.position.x;
                    float y = this.transform.position.y - BlockManager.instance.findBlock[i].transform.position.y;

                    if (Mathf.Abs(x) <= 1.2f)
                    {
                        if (Mathf.Abs(y) <= 1.2f)
                        {
                            if (BlockManager.instance.findBlock[i] != null)
                            {
                                if (BlockManager.instance.findBlock[i].GetComponent<Block>().blockType != BlockType.coin)
                                {
                                    BlockManager.instance.findBlock[i].GetComponent<Block>().RemoveBlock(BlockType.none);
                                }
                                else
                                {
                                    BlockManager.instance.findBlock[i].GetComponent<Block>().RemoveBlock(BlockType.coin);
                                }
                            }
                        }
                    }
                }

                effects = Instantiate(eEffect[0]);
                effects.transform.position = this.transform.position;
                break;
            case BlockType.eraserY:
                SoundManager.instance.PlayEffectSound(4);

                GameManager.instance.AddCoin(1);
                GameManager.instance.gameData.coin -= 10;

                coinEffect = Instantiate(getCoin1);
                coinEffect.GetComponent<TweenPosition>().from = this.transform.position;
                coinEffect.GetComponent<TweenPosition>().to = this.transform.position + (Vector3.up * 2);

                for (int i = BlockManager.instance.findBlock.Count - 1; i >= 0; i--)
                {
                    if (this.transform.position.x == BlockManager.instance.findBlock[i].transform.position.x)
                    {
                        if (BlockManager.instance.findBlock[i] != null)
                        {
                            if (BlockManager.instance.findBlock[i].GetComponent<Block>().blockType != BlockType.coin)
                            {
                                BlockManager.instance.findBlock[i].GetComponent<Block>().RemoveBlock(BlockType.none);
                            }
                            else
                            {
                                BlockManager.instance.findBlock[i].GetComponent<Block>().RemoveBlock(BlockType.coin);
                            }
                        }
                    }
                }

                effects = Instantiate(eEffect[1]);
                effects.transform.position = this.transform.position;
                break;
            case BlockType.eraserX:
                SoundManager.instance.PlayEffectSound(5);

                GameManager.instance.AddCoin(1);
                GameManager.instance.gameData.coin -= 10;

                coinEffect = Instantiate(getCoin1);
                coinEffect.GetComponent<TweenPosition>().from = this.transform.position;
                coinEffect.GetComponent<TweenPosition>().to = this.transform.position + (Vector3.up * 2);

                for (int i = BlockManager.instance.findBlock.Count - 1; i >= 0; i--)
                {
                    float y = this.transform.position.y - BlockManager.instance.findBlock[i].transform.position.y;

                    if (Mathf.Abs(y) < .8f)
                    {
                        if (BlockManager.instance.findBlock[i] != null)
                        {
                            if (BlockManager.instance.findBlock[i].GetComponent<Block>().blockType != BlockType.coin)
                            {
                                BlockManager.instance.findBlock[i].GetComponent<Block>().RemoveBlock(BlockType.none);
                            }
                            else
                            {
                                BlockManager.instance.findBlock[i].GetComponent<Block>().RemoveBlock(BlockType.coin);
                            }
                        }
                    }
                }

                effects = Instantiate(eEffect[2]);
                effects.transform.position = this.transform.position;
                break;
            case BlockType.coin:
                SoundManager.instance.PlayEffectSound(6);

                GameManager.instance.AddCoin(50);

                coinEffect = Instantiate(getCoin2);
                coinEffect.GetComponent<TweenPosition>().from = this.transform.position;
                coinEffect.GetComponent<TweenPosition>().to = this.transform.position + (Vector3.up * 2);

                effects = Instantiate(eEffect[3]);
                effects.transform.position = this.transform.position + new Vector3(0f, 0f, -2f);
                break;
            case BlockType.none:
                GameManager.instance.AddCoin(1);

                coinEffect = Instantiate(getCoin1);
                coinEffect.GetComponent<TweenPosition>().from = this.transform.position;
                coinEffect.GetComponent<TweenPosition>().to = this.transform.position + (Vector3.up * 2);
                break;
        }

        BlockManager.instance.allBlock.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    IEnumerator Blink(BlockType _type)
    {
        lifeLabel.transform.localScale = new Vector3(1.3f, 1.3f, 1);

        GameObject effect = null;

        SoundManager.instance.PlayEffectSound(8);

        switch (_type)
        {
            case BlockType.normal:
                break;
            case BlockType.fire:
                effect = Instantiate(bEffect[0]);
                effect.transform.position = this.gameObject.transform.position + new Vector3(0f, 0f, -1.5f);
                break;
            case BlockType.eraserX:
                effect = Instantiate(bEffect[2]);
                effect.transform.position = this.gameObject.transform.position + new Vector3(0f, 0f, -1.5f);
                break;
            case BlockType.eraserY:
                effect = Instantiate(bEffect[1]);
                effect.transform.position = this.gameObject.transform.position + new Vector3(0f, 0f, -1.5f);
                break;
            case BlockType.coin:
                effect = Instantiate(bEffect[3]);
                effect.transform.position = this.gameObject.transform.position + new Vector3(0f, 0f, -1.5f);
                break;
        }

        yield return new WaitForSeconds(.05f);

        lifeLabel.transform.localScale = new Vector3(1, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag.Equals("Ball") && col.transform.GetComponent<Ball>().wall)
        {
            life--;

            StartCoroutine(Blink(this.blockType));

            if (life <= 0)
            {
                //destroy block
                RemoveBlock(this.blockType);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.tag.Equals("Wall"))
        {
            if (GameManager.instance.curGameState == GameState.SelectBallDirection)
            {
                if (GameManager.instance.oneMoreChance == 0)
                {
                    SoundManager.instance.PlayEffectSound(7);

                    DataManager.Instance.SaveBlodck();
                    DataManager.Instance.SaveData();

                    GameManager.instance.curGameState = GameState.GameOver;
                    GameManager.instance.oneMoreChance = 1;
                }
                else
                {
                    SoundManager.instance.PlayEffectSound(7);

                    DataManager.Instance.SaveBlodck();
                    DataManager.Instance.SaveData();

                    GameManager.instance.curGameState = GameState.GameEnd;
                }
            }
        }
    }
}
