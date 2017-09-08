using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : ObjectManager
{
    public static EnemyManager instance;

    public TouchManager touchManager;

    private float rendDeleyTime = 1f;

    //적 Tpye에따른 시간
    private float normalTime = 0f, speederTime = 0f, tankerTime = 0f, laserTime = 0f, circleTime = 0f;
    private float x, y;

    [SerializeField]
    private float maxRange, minRange;

    private List<GameObject> warningList = new List<GameObject>();
    private List<GameObject> damageLabelList = new List<GameObject>();

    public Camera uiCam;
    private SmoothCamera mainCam;
    
    
    public GameObject bossHpObj;
    public UISprite bossHpBar;
    public UISprite bossImage;

    public bool bossState = false;
    public float bossCurLife;
    public float bossTotalLife;


    private void Awake()
    {
        if (instance == null)
            instance = this;

        mainCam = GameObject.Find("Main Camera").GetComponent<SmoothCamera>();
    }

    private void Start()
    {
        MakeObjs(this.makeObj[0]);

        //Warning Mark
        for (int i = 0; i < 200; i++)
        {
            GameObject _warning = Instantiate(this.makeObj[1]);
            _warning.transform.parent = this.transform;
            _warning.SetActive(false);

            warningList.Add(_warning);
        }

        //Damage Label
        for (int i = 0; i < 200; i++)
        {
            GameObject _label = Instantiate(this.makeObj[2]);
            _label.transform.parent = this.transform;
            _label.SetActive(false);

            damageLabelList.Add(_label);
        }
    }

    public void InitEnemyData()
    {
        bossState = false;
        GameManager.instance.enemyRend = true;

        GameManager.instance.stageCurTime = 0f;

        normalTime = 0f;
        speederTime = 0f;
        tankerTime = 0f;
        laserTime = 0f;
        circleTime = 0f;

        bossHpObj.SetActive(false);
    }

    //Warning 오브젝트
    private GameObject GetWarning()
    {
        foreach (GameObject _warning in warningList)
        {
            if (!_warning.activeInHierarchy)
            {
                return _warning;
            }
        }

        return null;
    }

    public GameObject GetDamageLabel()
    {
        foreach(GameObject _label in damageLabelList)
        {
            if(!_label.activeInHierarchy)
            {
                return _label;
            }
        }

        return null;
    }

    //Enemy Render
    public void RendEnemy()
    {
        if (GameManager.instance.enemyRend)
        {
            //stage Time
            GameManager.instance.stageCurTime += Time.deltaTime;

            //현재 stage시간이 limit타임이 되면 더이상 적을 생성하지 않는다.
            if (GameManager.instance.stageCurTime >= GameManager.instance.stageLimitTime)
            {
                GameManager.instance.enemyRend = false;
            }

            normalTime += Time.deltaTime;
            speederTime += Time.deltaTime;
            tankerTime += Time.deltaTime;
            laserTime += Time.deltaTime;
            circleTime += Time.deltaTime;

            //Enemy 타입별로 시간비율설정
            if (GameManager.instance.stageNum <= 8)
            {
                if (normalTime >= rendDeleyTime - (GameManager.instance.stageNum * 0.1f))
                {
                    GameObject newEnemy = GetObj();

                    if (newEnemy)
                    {
                        //위치지정
                        SetRandomPos();
                        newEnemy.transform.position = new Vector3(x, y, 0);

                        newEnemy.GetComponent<Enemy>().SetType(EnemyType.normal);
                        newEnemy.SetActive(true);

                        normalTime = 0f;
                    }
                }
                if (speederTime >= (rendDeleyTime * 3f) - (GameManager.instance.stageNum * 0.1f))
                {
                    GameObject newEnemy = GetObj();

                    if (newEnemy)
                    {
                        //위치지정
                        SetRandomPos();
                        newEnemy.transform.position = new Vector3(x, y, 0);

                        newEnemy.GetComponent<Enemy>().SetType(EnemyType.speeder);
                        newEnemy.SetActive(true);

                        speederTime = 0f;
                    }
                }
                if (tankerTime >= (rendDeleyTime * 2f) - (GameManager.instance.stageNum * 0.1f))
                {
                    GameObject newEnemy = GetObj();

                    if (newEnemy)
                    {
                        //위치지정
                        SetRandomPos();
                        newEnemy.transform.position = new Vector3(x, y, 0);

                        newEnemy.GetComponent<Enemy>().SetType(EnemyType.tanker);
                        newEnemy.SetActive(true);

                        tankerTime = 0f;
                    }
                }
            }
            else
            {
                if (normalTime >= 0.2f)
                {
                    GameObject newEnemy = GetObj();

                    if (newEnemy)
                    {
                        //위치지정
                        SetRandomPos();
                        newEnemy.transform.position = new Vector3(x, y, 0);

                        newEnemy.GetComponent<Enemy>().SetType(EnemyType.normal);
                        newEnemy.SetActive(true);

                        normalTime = 0f;
                    }
                }
                if (speederTime >= 0.6f)
                {
                    GameObject newEnemy = GetObj();

                    if (newEnemy)
                    {
                        //위치지정
                        SetRandomPos();
                        newEnemy.transform.position = new Vector3(x, y, 0);

                        newEnemy.GetComponent<Enemy>().SetType(EnemyType.speeder);
                        newEnemy.SetActive(true);

                        speederTime = 0f;
                    }
                }
                if (tankerTime >= 0.4f)
                {
                    GameObject newEnemy = GetObj();

                    if (newEnemy)
                    {
                        //위치지정
                        SetRandomPos();
                        newEnemy.transform.position = new Vector3(x, y, 0);

                        newEnemy.GetComponent<Enemy>().SetType(EnemyType.tanker);
                        newEnemy.SetActive(true);

                        tankerTime = 0f;
                    }
                }
            }
          
            //Laser Type Enemy Rend
            if (laserTime >= rendDeleyTime * 10f)
            {
                SoundManager.instance.PlayEffectSound(12);

                int maxLineNum = 2;
                int minLineNum = 1;

                if(GameManager.instance.stageNum <= 4)
                {
                    minLineNum = 1;
                    maxLineNum = 2;
                }
                else if(GameManager.instance.stageNum <= 10)
                {
                    minLineNum = 2;
                    maxLineNum = 4;
                }
                else
                {
                    minLineNum = 3;
                    maxLineNum = 6;
                }
                
                int repeatNum = Random.Range(minLineNum, maxLineNum);     //줄 숫자
                int dirNum = Random.Range(1, 5);        //날아오는 방향

                for (int i = 0; i < repeatNum; i++)
                {
                    GameObject newEnemy = GetObj();

                    if (newEnemy)
                    {
                        if (dirNum == 1)
                        {
                            //위치지정, i=간격
                            x = player.transform.position.x + (i * 2) - repeatNum;
                            y = player.transform.position.y + 10f;

                            newEnemy.transform.position = new Vector3(x, y, 0);

                            newEnemy.GetComponent<Enemy>().SetType(EnemyType.laser);
                            newEnemy.GetComponent<Enemy>().laserTarget = player.transform.position + new Vector3(i * 2 - repeatNum, -y, 0);

                            for (int j = -20; j < 0; j++)
                            {
                                GameObject warning = GetWarning();

                                if (warning != null)
                                {
                                    warning.transform.position = new Vector3(x, y + j, 0);

                                    warning.GetComponent<TweenScale>().ResetToBeginning();
                                    warning.GetComponent<TweenScale>().delay = -0.02f * j;
                                    warning.GetComponent<TweenScale>().from = new Vector3(0, 0, 1);
                                    warning.GetComponent<TweenScale>().to = new Vector3(0.5f, 0.5f, 1);
                                    warning.GetComponent<TweenScale>().Play();
                                    warning.SetActive(true);
                                }
                            }
                            newEnemy.SetActive(true);
                        }
                        else if (dirNum == 2)
                        {
                            //위치지정, i=간격
                            x = player.transform.position.x + (i * 2) - repeatNum;
                            y = player.transform.position.y - 10f;

                            newEnemy.transform.position = new Vector3(x, y, 0);

                            newEnemy.GetComponent<Enemy>().SetType(EnemyType.laser);
                            newEnemy.GetComponent<Enemy>().laserTarget = player.transform.position + new Vector3(i * 2 - repeatNum, -y, 0);

                            for (int j = -20; j < 0; j++)
                            {
                                GameObject warning = GetWarning();

                                if (warning != null)
                                {
                                    warning.transform.position = new Vector3(x, y - j, 0);

                                    warning.GetComponent<TweenScale>().ResetToBeginning();
                                    warning.GetComponent<TweenScale>().delay = -0.02f * j;
                                    warning.GetComponent<TweenScale>().Play();
                                    warning.SetActive(true);
                                }
                            }
                            newEnemy.SetActive(true);
                        }
                        else if (dirNum == 3)
                        {
                            //위치지정, i=간격
                            x = player.transform.position.x + 20f;
                            y = player.transform.position.y + (i * 2) - repeatNum;

                            newEnemy.transform.position = new Vector3(x, y, 0);

                            newEnemy.GetComponent<Enemy>().SetType(EnemyType.laser);
                            newEnemy.GetComponent<Enemy>().laserTarget = player.transform.position + new Vector3(-x, i * 2 - repeatNum, 0);

                            for (int j = -40; j < 0; j++)
                            {
                                GameObject warning = GetWarning();

                                if (warning != null)
                                {
                                    warning.transform.position = new Vector3(x + j, y, 0);

                                    warning.GetComponent<TweenScale>().ResetToBeginning();
                                    warning.GetComponent<TweenScale>().delay = -0.02f * j;
                                    warning.GetComponent<TweenScale>().Play();

                                    warning.SetActive(true);
                                }
                            }
                            newEnemy.SetActive(true);
                        }
                        else if (dirNum == 4)
                        {
                            //위치지정, i=간격
                            x = player.transform.position.x - 20f;
                            y = player.transform.position.y + (i * 2) - repeatNum;

                            newEnemy.transform.position = new Vector3(x, y, 0);

                            newEnemy.GetComponent<Enemy>().SetType(EnemyType.laser);
                            newEnemy.GetComponent<Enemy>().laserTarget = player.transform.position + new Vector3(-x, i * 2 - repeatNum, 0);

                            for (int j = -40; j < 0; j++)
                            {
                                GameObject warning = GetWarning();

                                if (warning != null)
                                {
                                    warning.transform.position = new Vector3(x - j, y, 0);

                                    warning.GetComponent<TweenScale>().ResetToBeginning();
                                    warning.GetComponent<TweenScale>().delay = -0.02f * j;
                                    warning.GetComponent<TweenScale>().Play();
                                    warning.SetActive(true);
                                }
                            }
                            newEnemy.SetActive(true);
                        }
                    }
                    if (i == repeatNum - 1)
                    {
                        laserTime = 0f;
                    }
                }
            }
            //Circle Type Enemy Rend
            if (circleTime >= rendDeleyTime * 16f)
            {
                SoundManager.instance.PlayEffectSound(12);

                SetRandomPos();

                for (int i = 0; i < 30; i++)
                {
                    GameObject newEnemy = GetObj();
                   
                    if(i == 0)
                    {
                        GameObject _warning = GetWarning();

                        if(_warning)
                        {
                            _warning.transform.position = new Vector3(x, y, 0) 
                                + ((player.transform.position - new Vector3(x, y, 0)) * 0.3f);

                            _warning.GetComponent<TweenScale>().ResetToBeginning();
                            _warning.GetComponent<TweenScale>().from = new Vector3(0, 0, 1);
                            _warning.GetComponent<TweenScale>().to = new Vector3(2f, 2f, 1);
                            _warning.GetComponent<TweenScale>().Play();
                            _warning.SetActive(true);
                        }
                    }
                    else
                    {
                        if (newEnemy)
                        {
                            newEnemy.transform.position
                                = newEnemy.GetComponent<Enemy>().circleStandard
                                = new Vector3(x , y, 0) + ((player.transform.position - new Vector3(x,y,0)) * 0.3f);
                            newEnemy.GetComponent<Enemy>().SetType(EnemyType.circle);
                            newEnemy.GetComponent<Enemy>().circleDelay = (i + 10) * .3f;
                            newEnemy.GetComponent<Enemy>().renderer.enabled = false;
                            newEnemy.SetActive(true);
                        }
                    }
                
                    if(i == 29)
                    {
                        circleTime = 0f;
                    }
                }
            }
        }

        //Boss
        else
        {
            bossHpBar.fillAmount = bossCurLife / bossTotalLife;
            if (!bossState)
            {
                GameObject newEnemy = GetObj();

                if (newEnemy)
                {
                    SoundManager.instance.PlayEffectSound(15);

                    //보스 HpBar
                    bossHpObj.GetComponent<TweenPosition>().ResetToBeginning();
                    bossHpObj.GetComponent<TweenPosition>().Play();
                    bossHpObj.SetActive(true);

                    mainCam.OnBlur();

                    //위치지정
                    SetRandomPos();
                    newEnemy.transform.position = new Vector3(x, y, 0);

                    newEnemy.GetComponent<Enemy>().SetType(EnemyType.boss);
                    newEnemy.SetActive(true);

                    //bossHpBar Boss Image
                    switch (newEnemy.GetComponent<Enemy>().bossImageNum)
                    {
                        case 0:
                            bossImage.spriteName = "boss1";
                            break;
                        case 1:
                            bossImage.spriteName = "boss2";
                            break;
                        case 2:
                            bossImage.spriteName = "boss3";
                            break;
                        case 3:
                            bossImage.spriteName = "boss4";
                            break;
                        case 4:
                            bossImage.spriteName = "boss5";
                            break;
                        case 5:
                            bossImage.spriteName = "boss6";
                            break;

                    }

                    bossState = true;
                }
            }
            //보스 등장 시 라인 줄어들음
            if (touchManager.limitDistance >= 3)
            {
                touchManager.limitDistance -= Time.deltaTime;
            }
            //stage 시간이 다 지났을 때 적을 검사
            if (!CheckObj())
            {
                //Next Stage
                GameManager.instance.enemyRend = true;

                bossState = false;

                touchManager.limitDistance = 30f;

                GameManager.instance.stageNum++;
                GameManager.instance.stageCurTime = 0f;
                GameManager.instance.stageLimitTime += 5f;

                //StarItem
                ItemManager.instance.RendStarItem(Random.Range(5,GameManager.instance.stageLimitTime));

                bossHpObj.SetActive(false);

                //적이 없을시(스테이지 클리어시) ready 상태로 전환
                GameManager.instance.StateTransition(GameState.ready);
            }
        }
    }
    
    //적이 남아있는지 체크
    private bool CheckObj()
    {
        return GameObject.FindWithTag("Enemy");
    }

    //x,y 위치지정
    private void SetRandomPos()
    {
        x = Random.Range(player.transform.position.x - maxRange, player.transform.position.x + maxRange);
        y = Random.Range(player.transform.position.y - maxRange, player.transform.position.y + maxRange);

        while (Mathf.Abs(x - player.transform.position.x) <= minRange &&
                Mathf.Abs(y - player.transform.position.y) <= minRange)
        {
            x = Random.Range(player.transform.position.x - maxRange, player.transform.position.x + maxRange);
            y = Random.Range(player.transform.position.y - maxRange, player.transform.position.y + maxRange);
        }
    }
}
