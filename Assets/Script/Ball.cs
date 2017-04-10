using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public enum ballState
    {
        ready,
        fire,
        none
    }
    public ballState thisBallState = ballState.ready;

    float ballSpeed = 8f;
    float fireTime = 0f;
    public float delayTime;
    Rigidbody2D rd;

    public bool bFireState = false;
    bool gatherState = false;
    bool goSelect = false;

    public bool wall = false;

    public bool potal = false;

    void Start()
    {
        delayTime = .5f;
        rd = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //발사딜레이
        if (bFireState)
        {
            fireTime += Time.deltaTime;
            if (fireTime >= delayTime)
            {
                FireBall(BallManager.instance.fireVec);
                
                bFireState = false;
                fireTime = 0f;
            }
        }

        //공모으기
        if (gatherState && GameManager.instance.curGameState == GameState.Fire)
        {
            GatherBall();
        }

        if (goSelect && GameManager.instance.curGameState == GameState.Fire)
        {
            //Debug.Log("1 goSelect true? : " + goSelect);

            //마지막 공이 모두 발사 되었는지
            if (BallManager.instance.fireNum == BallManager.instance.last)
            {
                //Debug.Log("2 fireNum == last ? : " + BallManager.instance.fireNum + " == " + BallManager.instance.last);

                if (BallManager.instance.setVec == this.transform.position)
                {
                    //Debug.Log("3 setVec == position ? : " + BallManager.instance.setVec + " == " + this.transform.position);

                    if (BallManager.instance.fireNum == BallManager.instance.backNum)
                    {
                       // Debug.Log("4 fireNum == backNum ? : " + BallManager.instance.fireNum + " == " + BallManager.instance.backNum);

                        BallManager.instance.fireNum = 0;
                        BallManager.instance.backNum = 0;

                        //Add Wave
                        GameManager.instance.AddWave();

                        GameManager.instance.curGameState = GameState.SelectBallDirection;
                        goSelect = false;
                    }
                }
            }
        }

        //이부분
        if(this.thisBallState == ballState.none){
            this.transform.position = BallManager.instance.setVec;
        }
        if(this.thisBallState == ballState.ready &&
            GameManager.instance.curGameState != GameState.Fire)
        {
            this.transform.position = BallManager.instance.setVec;
        }
    }

    public void FireBall(Vector2 vec)
    {
        rd.velocity = vec * ballSpeed;
        BallManager.instance.fireNum++;
        this.thisBallState = ballState.fire;
        this.potal = false;
        wall = true;

    }
    void GatherBall()
    {
        //move to firstball
        this.transform.position = Vector3.MoveTowards(this.transform.position,
                                                    BallManager.instance.setVec,
                                                    Time.deltaTime * 10f);

        if (this.transform.position == BallManager.instance.setVec
            && this.thisBallState == ballState.fire)
        {
            //wall check ball
            BallManager.instance.backNum++;

            //Change State
            if (BallManager.instance.fireNum == BallManager.instance.backNum)
            {
                goSelect = true;
                wall = false;
            }

            gatherState = false;
            this.thisBallState = ballState.none;
        }

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (GameManager.instance == null)
            GameManager.instance = GameObject.Find("@GameManager").GetComponent<GameManager>();

        if (GameManager.instance.curGameState == GameState.Fire)
        {
            if (Mathf.Abs(rd.velocity.y) < 0.3f)
            {
                rd.velocity = new Vector2(rd.velocity.x, 1.5f);
            }

            if (col.transform.tag.Equals("Wall"))
            {
                wall = false;
                //Stop Ball
                rd.velocity = Vector2.zero;

                //yPos Set
                this.transform.position = new Vector3(this.transform.position.x,
                                                        4.8f,
                                                        0f);

                //firstBall Save
                if (BallManager.instance.firstBall == null && this.thisBallState == ballState.fire)
                {
                    BallManager.instance.firstBall = this.gameObject;

                    //Set Position
                    BallManager.instance.SetPos(this.transform.position);
                    BallManager.instance.backNum++;
                    this.thisBallState = ballState.none;
                }
                //Is not firstBall
                else if (BallManager.instance.firstBall != null)
                {
                    //collect ball
                    gatherState = true;
                }

                BallManager.instance.movement = false;

                //Change State
                if (BallManager.instance.fireNum == BallManager.instance.backNum)
                {
                    goSelect = true;
                    wall = false;
                }
            }
        }
    }

}
