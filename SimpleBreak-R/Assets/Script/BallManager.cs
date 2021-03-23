using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallManager : MonoBehaviour
{
    public static BallManager instance;

    public Vector3 fireVec;        //발사벡터
    public Vector3 setVec;

    [SerializeField]
    float dx, dy;            //벡터x,y
    float time;
    float pendulumSpeed;         //진자운동 속도

    public int fireNum = 0;
    public int backNum = 0;

    public GameObject pendulum;  //발사표시오브젝트
    public GameObject firstBall = null;

    public bool movement;
    bool fireState, lastFire;

    [SerializeField]
    private GameObject ballPrefab;
    [SerializeField]
    private List<GameObject> ballList = new List<GameObject>();

    public int last;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        DataManager.Instance.first++;
        DataManager.Instance.SaveData();

        pendulumSpeed = 3.0f;
        fireVec = Vector3.zero;

        movement = false;
        fireState = false;

        SoundManager.instance.PlayEffectSound(1);
    }

    //시작 포지션
    public void SetPos(Vector3 pos)
    {
        setVec = pos;
    }

    //공 생성
    public void MakeBalls()
    {
        GameObject newBall = Instantiate(ballPrefab);
        newBall.transform.parent = this.transform;
        newBall.transform.localPosition = new Vector3(0f, 0f, 0f);
        newBall.transform.gameObject.SetActive(false);

        ballList.Add(newBall);
    }
    public void ActiveBall()
    {
        foreach (GameObject cball in ballList)
        {
            if (firstBall != null)
            {
                cball.transform.position = firstBall.transform.position;
            }
            cball.SetActive(true);
        }
    }
    public void ActiveLoadBall()
    {
        foreach (GameObject cball in ballList)
        {
            SetPos(new Vector3(PlayerPrefs.GetFloat("BALLPOSX"),
                                                    PlayerPrefs.GetFloat("BALLPOSY"),
                                                    0f));

            cball.transform.position = new Vector3(PlayerPrefs.GetFloat("BALLPOSX"),
                                                    PlayerPrefs.GetFloat("BALLPOSY"),
                                                    0f);
            cball.SetActive(true);
        }
    }


    //공 방향 설정
    public void SelectBallDirection()
    {
        if (!movement)
        {
            //방향 표시
            pendulum.transform.position = setVec;
            pendulum.SetActive(true);

            time += Time.deltaTime * pendulumSpeed;

            dx = Mathf.Sin(time) + setVec.x;
            dy = -Mathf.Abs(Mathf.Cos(time));


            if (dy >= -.25f)
                dy = -.25f;

            if (pendulum.activeInHierarchy)
                pendulum.transform.localPosition = new Vector3(dx, dy, 0f);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
            {
                if (hit.collider.tag.Equals("Background"))
                {
                    foreach (GameObject _ball in ballList)
                    {
                        _ball.GetComponent<Ball>().thisBallState = Ball.ballState.ready;
                    }

                    //activeBalls
                    ActiveBall();

                    //firstball init
                    firstBall = null;

                    //fireDirection normalize
                    fireVec = new Vector3(dx - setVec.x, dy, 0f);
                    fireVec.Normalize();

                    time = 0f;
                    movement = true;

                    //changeState
                    GameManager.instance.curGameState = GameState.MakeBlockCard;

                    last = ballList.Count;
                }
            }
        }
    }

    //상태변환
    public void FireStateChange()
    {
        fireState = true;

        GameManager.instance.curGameState = GameState.Fire;
    }

    //발사
    public void FireBalls()
    {
        int delayNum = 1;

        pendulum.SetActive(false);

        foreach (GameObject ball in ballList)
        {
            //시간값을 주고 공 발사
            ball.GetComponent<Ball>().delayTime = (.1f * delayNum);
            ball.GetComponent<Ball>().bFireState = true;
            delayNum++;
        }

        //발사 끝나면 false
        fireState = false;
    }

    private void Update()
    {
        if (fireState)
        {
            FireBalls();
        }
    }
}
