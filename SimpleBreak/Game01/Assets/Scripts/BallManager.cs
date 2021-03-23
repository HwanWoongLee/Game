using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallManager : MonoBehaviour
{
    public static BallManager instance;

    public GameObject ballPrefab;       //공프리팹.
    public GameObject arrow;            //화살표시.

    public List<GameObject> balls = new List<GameObject>();     //공들.

    public Vector3 setPos;          //시작위치.
    private Vector3 mousePos;       //마우스위치.
    private Vector3 moveVec;        //공이 움직이는 방향.

    private float curTime = 0f;
    private float delayTime = .02f;  //볼나가는 딜레이타임.

    public GameObject firstBall = null;

    public bool once;       //처음 공 하나.
    private bool clickState = false;

    public int checkNum = 0;
    public int throwNum = 0;
    public int lastNum = 0;

    public GameObject sphereCast;
    public GameObject lineCast;
    public GameObject withdraw;

    bool clickCheck = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        once = false;
        if (DataManager.Instance.FirstDecide())
            FirstStartSet();
        else
        {
            LoadStartSet();
        }
    }

    void FirstStartSet()
    {
        SettingSetPos(new Vector3(0f, -4.45f, 0f));      //처음시작 위치.
        MakeBall();                                     //처음 공 하나 생성.
        balls[0].SetActive(true);
    }
    void LoadStartSet()
    {
        SettingSetPos(new Vector3(PlayerPrefs.GetFloat("BALLX"), PlayerPrefs.GetFloat("BALLY"), 0f));      //처음시작 위치.
        for (int i = 0; i < GameManager.instance.ballNum; i++)
        {
            MakeBall();
            balls[i].transform.position = setPos;
            balls[i].SetActive(true);
        }
    }

    //클릭체크.
    void CheckClick()
    {
        if (Input.GetMouseButton(0) && clickState)
        {
            once = false;
            clickCheck = false;
            checkNum = 0;
            lastNum = 0;
            throwNum = GameManager.instance.ballNum;

            //터치 위치를 저장.
            mousePos = Camera.main.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

            //방향설정.
            moveVec = mousePos - setPos;
            moveVec.Normalize();

            //각도 한계.
            if (moveVec.x >= .95f)
                moveVec.x = .95f;
            else if (moveVec.x <= -.95f)
                moveVec.x = -.95f;
            if (moveVec.y <= .25f)
                moveVec.y = .25f;

            //firstBall
            if (firstBall == null)
            {
                firstBall = GameObject.FindWithTag("Ball");
            }

            //공을 ballNum만큼 활성화.
            for (int i = 0; i < GameManager.instance.ballNum; i++)
            {
                if (firstBall != null)
                {
                    balls[i].transform.position = firstBall.transform.position;
                }
                balls[i].GetComponent<Ball>().fire = -1;
                balls[i].SetActive(true);
            }

            //선그리기
            DrawLine();
        }
        else if (Input.GetMouseButtonUp(0) && clickState )
        {
            //표시없에줌.
            arrow.SetActive(false);
            sphereCast.SetActive(false);
            lineCast.SetActive(false);

            //상태 초기화.
            firstBall = null;
            clickState = false;
            Time.timeScale = 1f;

            //공 발사상태로.
            if (GameManager.instance.curGameState != GameManager.GameState.fire)
            {
                GameManager.instance.curGameState = GameManager.GameState.fire;
            }

            //공을 ballNum만큼 순서대로 발사해준다.
            for (int i = 0; i < GameManager.instance.ballNum; i++)
            {
                //공 발사 방향.
                balls[i].GetComponent<Ball>().SettingMoveVec(moveVec);

                balls[i].GetComponent<Ball>().SetDelayTime(i);
                balls[i].GetComponent<Ball>().fire = 1;
            }
        }
    }

    //시작위치시정.
    public void SettingSetPos(Vector3 pos)
    {
        setPos = pos;
    }

    //볼생성.
    public void MakeBall()
    {
        GameObject newball = Instantiate(ballPrefab);
        newball.transform.parent = this.transform;
        newball.transform.position = new Vector3(0, -4.5f, 0);
        newball.SetActive(false);

        balls.Add(newball);
    }

    //회수버튼.
    public void WithdrawBalls()
    {
        if (once && !clickCheck && GameManager.instance.curGameState == GameManager.GameState.fire)
        {
            SoundManager.instance.PlaySound(2);

            foreach (GameObject ball in balls)
            {
                ball.GetComponent<Ball>().Stop();

                ball.GetComponent<Ball>().gatherState = true;
            }
            
            BallManager.instance.throwNum = BallManager.instance.checkNum;

            GameManager.instance.curGameState = GameManager.GameState.moveblock;
            GameManager.instance.AddWave();                 //레벨추가.
            BlockManager.instance.ArrangeBlock();           //블록배치.
            ItemManager.instance.ArrangeItem();             //아이템배치.

            clickCheck = true;
        }
    }

    //궤적 그리기.
    void DrawLine()
    {
        //화살표 표시.
        arrow.transform.position = firstBall.transform.position;
        arrow.transform.rotation = Quaternion.LookRotation(moveVec);

        RaycastHit hit;

        //SphereCast로 충돌검사.
        bool isHit = Physics.SphereCast(setPos, ballPrefab.transform.localScale.x / 2, moveVec, out hit, 20f);

        
        if (isHit)
        {
            lineCast.SetActive(true);
            lineCast.transform.position = firstBall.transform.position;
            lineCast.transform.localRotation = Quaternion.LookRotation(moveVec);
        }
        else
        {
            lineCast.SetActive(true);
            lineCast.transform.position = firstBall.transform.position;
            lineCast.transform.localRotation = Quaternion.LookRotation(moveVec);
        }
    }

    void Update()
    {
        if (GameManager.instance.curGameState == GameManager.GameState.ready
            )
        {
            CheckClick();
            withdraw.SetActive(false);

        }

        //보드 위에서만 클릭 되게.
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitObj;

            if (Physics.Raycast(ray, out hitObj, Mathf.Infinity))
            {
                if (hitObj.transform.tag.Equals("Board"))
                {
                    clickState = true;
                }
            }
        }

        //공모으기 조건.
        if(GameManager.instance.curGameState == GameManager.GameState.fire
            && once
            && lastNum >= throwNum)
        {
            withdraw.SetActive(true);
        }
    }
}
