using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour
{
    public enum BlockType
    {
        normal,     //0
        fire,       //1
        eraser,     //2
        none
    }

    public BlockType thisBlockType;      //블록타입.

    public UILabel lifeNum;             //블록라이프 라벨.

    public int blockLife;               //라이프.

    private Vector3 targetPos;
    private bool moveState;

    public Sprite[] imageType;
    private SpriteRenderer image;

    public GameObject[] effect = new GameObject[3];

    public GameObject sEffect, fEffect;
 
    public UIPanel label;

    private AudioSource audio;
    public AudioClip sound;

    void Start()
    {
        audio = this.GetComponent<AudioSource>();

        lifeNum = GetComponentInChildren<UILabel>();
        image = this.transform.FindChild("Image").GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (lifeNum != null)
            lifeNum.text = blockLife.ToString();

        if (GameManager.instance.curGameState == GameManager.GameState.gameover
            || GameManager.instance.curGameState == GameManager.GameState.end)
        {
            lifeNum.text = "";
        }

        ChangeImage();
        HeightCheck();

        if (blockLife <= 0)
        {
            StartCoroutine(RemoveBlock(this.thisBlockType));
        }

    }

    //충돌체크
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag.Equals("Ball") && GameManager.instance.curGameState == GameManager.GameState.fire)
        {
            SubtractLife();
            //깜빡임.
            StartCoroutine(Blink(this.thisBlockType));
        }
    }

    //공에 맞으면 라이프 감소.
    void SubtractLife()
    {
        if (GameManager.instance.curGameState == GameManager.GameState.fire)
        {
            audio.PlayOneShot(sound);

            if (blockLife>=0)
                blockLife--;
        }
    }
    IEnumerator Blink(BlockType _type)
    {
        label.transform.localScale = new Vector3(2.3f, 1.3f, 1);

        GameObject effect = null;

        switch (_type)
        {
            case BlockType.normal:
                break;
            case BlockType.fire:
                //불블록.
                effect = Instantiate(fEffect);
                effect.transform.position = this.gameObject.transform.position + new Vector3(0f,0f,-1.5f);
                break;
            case BlockType.eraser:
                //지우개블록.
                effect = Instantiate(sEffect);
                effect.transform.position = this.gameObject.transform.position + new Vector3(0f, 0f, -1.5f);
                break;
        }
        yield return new WaitForSeconds(.05f);

        label.transform.localScale = new Vector3(2, 1, 1);

        if (effect != null)
            effect.SetActive(false);
    }

    //블록내리기.
    public void FallBlock()
    {
        label.transform.localScale = new Vector3(2, 1, 1);

        if (!moveState)
        {
            targetPos = transform.position + Vector3.down;
            moveState = true;
        }
        else if(moveState)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                                 targetPos,
                                                 2f * Time.deltaTime);
            
            if (transform.position == targetPos)
            {
                GameManager.instance.curGameState = GameManager.GameState.ready;
                moveState = false;
            }
        }
    }

    //블록 제거.
    IEnumerator RemoveBlock(BlockType _type)
    {
        yield return new WaitForSeconds(0f);     //바로 없어지면 공이 튕기지 않기 떄문에 딜레이.

        switch (_type)
        {
            case BlockType.normal:
                //노말블록.
                Instantiate(effect[0], this.transform.position, Quaternion.Euler(new Vector3(36.5f,180f,180f)));
                SoundManager.instance.PlaySound(3);

                break;
            case BlockType.fire:
                SoundManager.instance.PlaySound(7);

                //불블록.
                Instantiate(effect[1], this.transform.position, this.transform.localRotation);

                BlockManager.instance.Search();
                foreach (GameObject _block in BlockManager.instance.findBlocks)
                {
                    float x = this.transform.position.x - _block.transform.position.x;
                    float y = this.transform.position.y - _block.transform.position.y;

                    if (Mathf.Abs(x) <= 1f)
                    {
                        if (Mathf.Abs(y) <= 1f)
                        {
                            if (_block != null)
                            {
                                _block.GetComponent<Block>().thisBlockType = BlockType.none;
      
                                _block.GetComponent<Block>().blockLife = 0;
                            }
                        }
                    }
                }
          

                break;
            case BlockType.eraser:
                SoundManager.instance.PlaySound(6);

                //지우개블록.
                Instantiate(effect[2], this.transform.position, this.transform.localRotation);

                BlockManager.instance.Search();
                foreach (GameObject _block in BlockManager.instance.findBlocks)
                {
                    if (this.transform.position.x == _block.transform.position.x)
                    {
                        if (_block != null)
                        {
                            _block.GetComponent<Block>().thisBlockType = BlockType.none;

                            _block.GetComponent<Block>().blockLife = 0;
                        }
                    }
                }
            
                break;
        }
        this.gameObject.SetActive(false);
    }

    //블록 높이 체크.
    private void HeightCheck()
    {
        //오브젝트가 활성화 상태라면.
        if (this.gameObject.activeInHierarchy)
        {
            if (this.transform.position.y <= -4f)
            {
                //게임오버.
                GameManager.instance.gameOverCheck = 1;
                
                //게임엔드.
                if (GameManager.instance.oneMoreCheck == 1)
                    GameManager.instance.curGameState = GameManager.GameState.end;
            }
        }
    }

    //이미지를 바꿔줌.
    private void ChangeImage()
    {
        switch (thisBlockType)
        {
            case BlockType.normal:
                image.sprite = imageType[0];
                break;
            case BlockType.fire:
                image.sprite = imageType[1];
                break;
            case BlockType.eraser:
                image.sprite = imageType[2];
                break;
        }
    }
}
