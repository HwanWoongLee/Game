using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    enum ballState { READY, MOVE }
    ballState curBallState = ballState.READY;

    private float moveSpeed = 10f;      //볼 스피드.

    float delayTime;
    float curTime = 0f;

    private Vector3 movePos;
    private Vector3 stopPos;

    public int fire;                //-1 발사전, 1 발사중, 0 발사후

    public bool gatherState;
    private bool potalState;        //포탈 했는지 판명.

    public GameObject coinEffect, plusItemEffect;

    void Start()
    {
        fire = -1;
        gatherState = false;
        potalState = false;

        movePos = Vector3.zero;
    }

    //무브벡터설정.
    public void SettingMoveVec(Vector3 vec)
    {
        movePos = vec;
    }

    //발사.
    public void Fire()
    {
        if (curBallState == ballState.MOVE)
            return;

        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = movePos * moveSpeed;      //방향, 속도.

        BallManager.instance.lastNum++;

        curBallState = ballState.MOVE;          //볼상태 move.
        GameManager.instance.curGameState = GameManager.GameState.fire;

        gatherState = false;
        potalState = false;
    }

    //공멈춤.
    public void Stop()
    {
        if (curBallState == ballState.MOVE)
        {
            curTime = 0f;

            BlockManager.instance.InitBlock();

            Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
            rb.velocity = Vector3.zero;

            fire = 0;
            curBallState = ballState.READY;
        }
    }

    public void SetDelayTime(int i)
    {
        delayTime = (float)(i * 0.1f);
    }

    //공 모으기.
    private void Gather()
    {
        if (BallManager.instance.firstBall != null && fire == 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, BallManager.instance.firstBall.transform.position, .5f);
        }
    }

    //충돌체크
    private void OnCollisionEnter2D(Collision2D col)
    {
        //바닥에 충돌시.
        if (col.transform.tag.Equals("Bottom") && fire == 1)
        {
            BallManager.instance.checkNum++;        //몇번째 충돌인지 검사.

            //모든 공의 운동을 멈춤.
            Stop();

            //첫번째 공이 아닐경우 첫번째 공의 자리로 이동.
            if (BallManager.instance.firstBall != null  )
                gatherState = true;

            //첫번째 공일경우 검사.
            if (!BallManager.instance.once)
            {
                BallManager.instance.firstBall = this.transform.gameObject;  //첫번째 공 전달.
                this.transform.position = new Vector3(this.transform.position.x, -4.34f, this.transform.position.z);

                BallManager.instance.SettingSetPos(transform.position);
                BallManager.instance.once = true;
            }

            //wave종료지점!!!!!!!!!!!!!!!!!!!!!

            //충돌 수가 던진 공의 수와 같을 시(마지막 공이 바닥에 충돌시)
            if (BallManager.instance.checkNum == BallManager.instance.throwNum)
            {
                GameManager.instance.curGameState = GameManager.GameState.moveblock;
                GameManager.instance.AddWave();                 //레벨추가.
                BlockManager.instance.ArrangeBlock();           //블록배치.
                ItemManager.instance.ArrangeItem();             //아이템배치.

                //포탈이 발동했으면 다음웨이브에서 지워줌.
                if (potalState)
                {
                    ItemManager.instance.outPotal.SetActive(false);
                    ItemManager.instance.inPotal.SetActive(false);
                    ItemManager.instance.potalMade = 0;
                }
            }
        }
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag.Equals("ItemPotalIn") && !potalState)
        {
            if (ItemManager.instance.outPotal.activeInHierarchy)
            {
                SoundManager.instance.PlaySound(1);

                this.gameObject.transform.position = ItemManager.instance.outPotal.transform.position;
                potalState = true;
            }
        }

        else if (other.transform.tag.Equals("ItemPlusBall"))
        {
            //플러스아이템 먹기.
            BallManager.instance.MakeBall();
            GameManager.instance.AddBall();
            other.transform.gameObject.SetActive(false);

            GameObject effect = Instantiate(plusItemEffect);
            effect.GetComponent<TweenPosition>().from = other.transform.position + new Vector3(0f, .5f, -2f);
            effect.GetComponent<TweenPosition>().to = other.transform.position + Vector3.up;

        }
        else if (other.transform.tag.Equals("Coin"))
        {
            //코인 먹기.
            GameManager.instance.AddCoin();
            other.transform.gameObject.SetActive(false);

            GameObject effect = Instantiate(coinEffect);
            effect.GetComponent<TweenPosition>().from = other.transform.position + new Vector3(0f,.5f,-2f);
            effect.GetComponent<TweenPosition>().to = other.transform.position + Vector3.up;

        }
    }
    void FixedUpdate()
    {
        if (fire == 1)
        {
            curTime += Time.deltaTime;
            if (curTime >= delayTime)
            {
                Fire();
            }
        }

        if (gatherState)
            Gather();
    }
}
