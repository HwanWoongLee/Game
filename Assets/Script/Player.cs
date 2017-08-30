using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    //Damage
    public int Damage = 1;
    //MoveSpeed
    public float moveSpeed = 3;
    //FireSpeed
    public float bulletDelayTime = .1f;

    public int powerNum = 1;
    public float powerGauge = 0f;
    public float maxGauge = 100f;

    public float limit;
    private bool gaugeState = false;

    public float curTime = 0f;

    //방향 vector
    public Vector3 lookDirection;
    public Vector3 fireDirection;

    [SerializeField]
    private GameObject getEffect;

    //Speed 버프
    public bool getSpeedItem = false;
    public float speedTime = 0;
    //자석버프
    public bool getMagnetItem = false;
    public float magnetTime = 0;

    private SmoothCamera blurCam;

    //플레이어 타입 번호
    public int playerType = 0;
    public Sprite[] playerImage = new Sprite[7];
    public GameObject[] playerSprite = new GameObject[2];

    // DUMMYSYSTEM
    [SerializeField]
    private GameObject dummyObj;
    [SerializeField]
    private List<GameObject> dummyList = new List<GameObject>();

    public int dummyNum = 0;

    private float dx, dy;
    private float dummyRotTime = 0f;
    private float dummyRotSpeed = 4f;

    public GameObject masterEffect;
    public GameObject starEffect;
    public GameObject getCoinLabel;
 
    //플레이어 초기화
    public void InitPlayer()
    {
        //위치
        this.transform.position = Vector3.zero;

        //방향
        this.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));

        //이동, 발사속도,Dmage
        GetPlayerJsonData();

        //버프해제
        getMagnetItem = false;
        getSpeedItem = false;
        speedTime = 0f;
        magnetTime = 0f;

        for (int i = 0; i < powerNum; i++)
        {
            RemoveDummy();
        }

        //power level init
        gaugeState = false;
        powerGauge = 0;
        powerNum = 0;

        limit = 0;

        dummyNum = 0;

        this.gameObject.SetActive(false);
    }

    private void Awake()
    {
        fireDirection = Vector3.up;
        blurCam = GameObject.Find("Main Camera").GetComponent<SmoothCamera>();
    }

    private void Start()
    {
        GetPlayerJsonData();
    }

    private void Update()
    {
        RotateDummy();
        if (GameManager.instance.curGameState == GameState.game)
        {
            PlayerFired();

            #region 버프
            //Speed버프효과
            if (getSpeedItem)
            {
                speedTime += Time.deltaTime;
                if (speedTime >= 5f)
                {
                    moveSpeed -= 3f;
                    getSpeedItem = false;
                    speedTime = 0f;
                }
            }
            //Magnet버프
            if (getMagnetItem)
            {
                magnetTime += Time.deltaTime;
                if (magnetTime >= 10f)
                {
                    getMagnetItem = false;
                    magnetTime = 0f;
                }
            }

            //powerItem습득시
            if (gaugeState)
            {
                GetPowerItem();
            }
            else
            {
                if (powerGauge > 0)
                    powerGauge -= 3f * Time.deltaTime;

                //게이지가 최소로 줄어들면
                if (powerGauge <= 0 && powerNum > 0)
                {
                    RemoveDummy();
                    powerNum--;
                    maxGauge -= 10;
                    powerGauge = maxGauge - 30;
                }
            }
            #endregion
        }
    }

    //JsonData 불러 적용
    //type , level, data
    public void GetPlayerJsonData()
    {
        Damage = GameManager.instance.jsonData.LoadData(playerType,
            GameManager.instance.redLevel[playerType]).damage;

        //이동속도 고정으로 수정
        moveSpeed = 3;

        bulletDelayTime = GameManager.instance.jsonData.LoadData(playerType,
            GameManager.instance.blueLevel[playerType]).fireSpeed * 0.1f;

        //기존 이동속도 데이터를 총알크기로 수정
        BulletManager.instance.bulletScale = GameManager.instance.jsonData.LoadData(playerType,
            GameManager.instance.greenLevel[playerType]).moveSpeed;

        //능력치 마스터시 효과 활성화
        if (masterEffect != null)
        {
            if (GameManager.instance.redLevel[playerType] >= 4
            && GameManager.instance.greenLevel[playerType] >= 4
            && GameManager.instance.greenLevel[playerType] >= 4)
            {
                masterEffect.gameObject.SetActive(true);
            }
            else
            {
                masterEffect.gameObject.SetActive(false);
            }
        }
    }

    //발사
    private void PlayerFired()
    {
        if (this.transform.tag != "Player")
            return;

        curTime += Time.deltaTime;

        if (curTime >= bulletDelayTime)
        {
            BulletManager.instance.FireBullets(this.transform.position);

            //발사 사운드,데미지
            switch (BulletManager.instance.curBulletType)
            {
                case BulletManager.bulletType.normal:
                    SoundManager.instance.PlayEffectSound(4);
                    break;
                case BulletManager.bulletType.big:
                    SoundManager.instance.PlayEffectSound(5);
                    break;
                case BulletManager.bulletType.laser:
                    SoundManager.instance.PlayEffectSound(6);
                    break;
                case BulletManager.bulletType.bounce:
                    SoundManager.instance.PlayEffectSound(7);
                    break;
                case BulletManager.bulletType.guided:
                    SoundManager.instance.PlayEffectSound(8);
                    break;
                case BulletManager.bulletType.sword:
                    SoundManager.instance.PlayEffectSound(9);
                    break;
                case BulletManager.bulletType.explosion:
                    SoundManager.instance.PlayEffectSound(10);
                    break;
            }

            //더미리스트가 0이 아닐시 동시에 같이 발사해줌.
            if (dummyList.Count != 0)
            {
                foreach (GameObject _dummy in dummyList)
                {
                    BulletManager.instance.FireBullets(_dummy.transform.position);
                }
            }

            curTime = 0f;
        }
    }

    //파워아이템
    private void GetPowerItem()
    {
        //powerGauge get
        if (powerGauge <= limit)
        {
            powerGauge += 0.5f;
        }
        else
        {
            gaugeState = false;
        }

        //powerGauge full
        if (powerGauge >= maxGauge)
        {
            if (powerNum < 5)
            {
                PowerUp();
                powerGauge = 0f;
                limit = 20f;
                maxGauge += 10f;
            }
            else
                return;
        }
    }

    //기체 파워업
    private void PowerUp()
    {
        SoundManager.instance.PlayEffectSound(21);

        powerNum++;

        MakeDummy();
    }

    //플레이어 방향 전환
    public void SetPlayerDirection(Vector3 dir)
    {
        fireDirection = dir;
        lookDirection = this.transform.position + dir;
        this.transform.LookAt(lookDirection);
    }

    //비행기 타입 변경
    public void ChangePlayerType(int _typeNum)
    {
        this.playerType = _typeNum;

        //총알변경
        BulletManager.instance.SetBulletType(this.playerType);

        //Sprite Image변경
        playerSprite[0].GetComponent<SpriteRenderer>().sprite = playerImage[this.playerType];
        playerSprite[1].GetComponent<SpriteRenderer>().sprite = playerImage[this.playerType];

        foreach (GameObject _dummy in this.dummyList)
        {
            _dummy.GetComponent<Player>().ChangePlayerType(_typeNum);
        }

        //type에 따른 jsonData불러오기
        GetPlayerJsonData();
    }


    #region 더미관련
    //더미플레이어 추가
    public void MakeDummy()
    {
        if (dummyObj == null)
            return;

        dummyNum++;
        GameObject _dummy = Instantiate(dummyObj);
        _dummy.transform.parent = this.transform;
        _dummy.transform.localRotation = Quaternion.identity;

        //Sprite Image변경
        _dummy.GetComponent<Player>().playerSprite[0].GetComponent<SpriteRenderer>().sprite = playerImage[this.playerType];
        _dummy.GetComponent<Player>().playerSprite[1].GetComponent<SpriteRenderer>().sprite = playerImage[this.playerType];

        dummyList.Add(_dummy);
    }

    //더미플레이어 제거
    public void RemoveDummy()
    {
        if (dummyObj == null)
            return;

        dummyNum--;

        if (dummyNum < 0)
        {
            GameManager.instance.StateTransition(GameState.over);
        }

        if (dummyList.Count > 0)
        {
            //더미하나 삭제
            Destroy(dummyList[0]);
            dummyList.Remove(dummyList[0]);

            blurCam.OnBlur();
        }
    }

    //dummy회전
    private void RotateDummy()
    {
        dummyRotTime += Time.deltaTime * dummyRotSpeed;

        for (int i = 0; i < dummyNum; i++)
        {
            dx = Mathf.Cos(dummyRotTime + (i * (6.28f / dummyNum)));
            dy = Mathf.Sin(dummyRotTime + (i * (6.28f / dummyNum)));

            dummyList[i].transform.localPosition = new Vector3(0f, dy, dx);
        }
    }
    #endregion

    //충돌
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.curGameState != GameState.game
            && GameManager.instance.curGameState != GameState.ready)
            return;

        if (other.transform.tag.Equals("Bullet")
            || other.transform.tag.Equals("Warning")
            || other.transform.tag.Equals("BobmEffect")
            || other.transform.tag.Equals("Fever"))
            return;

        switch (other.transform.tag)
        {
            case "PowerUpItem":
                gaugeState = true;
                limit = powerGauge + 20f;
                GetItem(1);

                SoundManager.instance.PlayEffectSound(23);
                break;
            case "MoveItem":
                getSpeedItem = true;
                speedTime = 0f;
                moveSpeed += 3f;

                GetItem(0);

                SoundManager.instance.PlayEffectSound(20);
                break;
            case "MagnetItem":
                getMagnetItem = true;
                magnetTime = 0f;
                GetItem(2);

                SoundManager.instance.PlayEffectSound(22);
                break;
            //적충돌
            case "Enemy":
                //보스 충돌시 레벨에 관련없이 over
                if (other.transform.GetComponent<Enemy>().enemyType == EnemyType.boss)
                {
                    GameManager.instance.StateTransition(GameState.over);
                    break;
                }

                if (powerNum > 0)
                {
                    SoundManager.instance.PlayEffectSound(19);
                    powerGauge = 0;
                }
                else
                {
                    GameManager.instance.StateTransition(GameState.over);
                }
                break;
            case "GetBox":
                for (int i = 0; i < ItemManager.instance.boxItems.Count; i++)
                {
                    Destroy(ItemManager.instance.boxItems[i]);
                    if (i == ItemManager.instance.boxItems.Count - 1)
                    {
                        ItemManager.instance.boxItems.Clear();
                    }
                }

                switch (other.GetComponent<GetBox>().thisBoxType)
                {
                    case GetBox.boxType.normal:
                        SoundManager.instance.PlayEffectSound(21);

                        ChangePlayerType(0);
                        break;
                    case GetBox.boxType.big:
                        SoundManager.instance.PlayEffectSound(21);

                        ChangePlayerType(1);
                        break;
                    case GetBox.boxType.laser:
                        SoundManager.instance.PlayEffectSound(21);

                        ChangePlayerType(2);
                        break;
                    case GetBox.boxType.bounce:
                        SoundManager.instance.PlayEffectSound(21);

                        ChangePlayerType(3);
                        break;
                    case GetBox.boxType.guided:
                        SoundManager.instance.PlayEffectSound(21);

                        ChangePlayerType(4);
                        break;
                    case GetBox.boxType.sword:
                        SoundManager.instance.PlayEffectSound(21);

                        ChangePlayerType(5);
                        break;
                    case GetBox.boxType.explosion:
                        SoundManager.instance.PlayEffectSound(21);
                        ChangePlayerType(6);
                        break;
                    case GetBox.boxType.coin:
                        SoundManager.instance.PlayEffectSound(18);

                        int getCoinNum = (GameManager.instance.stageNum * Random.Range(30, 101));

                        GameManager.instance.coin += getCoinNum;

                        ShowGetCoin(getCoinNum,this.transform.position);
                        break;
                }

                GameManager.instance.StateTransition(GameState.game);
                break;
            case "StarItem":
                SoundManager.instance.PlayEffectSound(16);

                GameObject _starEffect = Instantiate(starEffect);
                _starEffect.transform.position = this.transform.position;
                _starEffect.GetComponent<TweenScale>().ResetToBeginning();
                _starEffect.GetComponent<TweenScale>().Play();
                _starEffect.SetActive(true);

                ItemManager.instance.RendFever();
                break;
        }

        other.transform.gameObject.SetActive(false);
    }

    private void ShowGetCoin(int _coin, Vector3 pos)
    {
        GameObject newlabel = Instantiate(getCoinLabel);

        newlabel.transform.localScale = new Vector3(1, 1, 1);
        newlabel.transform.position = pos;

        newlabel.transform.FindChild("label").GetComponent<UILabel>().text = "+" + _coin;

        newlabel.GetComponent<TweenPosition>().from = pos;
        newlabel.GetComponent<TweenPosition>().to = pos + (Vector3.up * 1f);

        Destroy(newlabel, 1f);
    }

    //아이템 획득
    void GetItem(int _sNum)
    {
        GameObject _effect = Instantiate(getEffect);
        _effect.GetComponent<GetEffect>().SetSprite(_sNum);
        _effect.transform.parent = this.transform;
        _effect.transform.localPosition = Vector3.zero;
        _effect.transform.position += Vector3.forward;
    }
}