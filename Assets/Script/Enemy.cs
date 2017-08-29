using UnityEngine;
using System.Collections;

public enum EnemyType
{
    normal,
    speeder,
    tanker,
    laser,
    boss,
    circle
}

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType = EnemyType.normal;

    private float moveSpeed = 1.5f;
    public float distance = 0f;

    //Sprite Renderer
    public new SpriteRenderer renderer;
    public Sprite[] enemyImage = new Sprite[3];
    public Sprite[] bossImage = new Sprite[6];
    public int bossImageNum;

    //Laser Pattern
    public Vector3 laserTarget;
    private Vector3 laserMoveVec;
    private float laserDelay = 0f;
    private bool aming = false;

    //circle Pattern
    public Vector3 circleStandard;
    public Vector3 circleTarget;
    public Vector3 circleMoveVec;
    public float circleTime;
    public float circleDelay;
    public float cx, cy;

    private Player player;

    //보스 체력
    public int Life;
    private int standardLife;

    public int tankerLife = 5;
    public int bossLife = 20;
    public int normalLife = 1;

    [SerializeField]
    private GameObject desEffect;
    [SerializeField]
    private GameObject damageLabel;
    [SerializeField]
    private GameObject getBox;
    
    public TrailRenderer trail;

    //Enemy활성화시
    private void OnEnable()
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        aming = false;
        laserDelay = 0f;
        circleTime = 0f;

        if (this.enemyType == EnemyType.boss)
        {
            Life = bossLife * (1 + GameManager.instance.stageNum);
            standardLife = Life;

            EnemyManager.instance.bossCurLife = Life;
            EnemyManager.instance.bossTotalLife = standardLife;
        }
        else if (this.enemyType == EnemyType.tanker)
        {
            Life = tankerLife * (1 + GameManager.instance.stageNum);
        }
        else
        {
            Life = normalLife * (1 + GameManager.instance.stageNum);
        }

        if (this.enemyType == EnemyType.laser
            || this.enemyType == EnemyType.circle)
        {
            this.trail.enabled = true;
        }
        else
        {
            this.trail.enabled = false;
        }
    }

    void Update()
    {
        MoveToPlayer();
    }

    //이동
    private void MoveToPlayer()
    {
        //레이저패턴
        if (this.enemyType == EnemyType.laser)
        {
            //조준
            if (!aming)
            {
                laserMoveVec = laserTarget - this.transform.position;
                laserMoveVec.Normalize();
                aming = true;
            }
            else
            {
                laserDelay += Time.deltaTime;
                if (laserDelay >= .5f)
                {
                    this.transform.position += laserMoveVec * Time.deltaTime * moveSpeed;
                }
                if (laserDelay >= 5f)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
        //원형 패턴
        else if (this.enemyType == EnemyType.circle)
        {
            circleTime += Time.deltaTime * 2;

            if (circleTime >= circleDelay)
            {
                this.renderer.enabled = true;
                this.transform.position += circleMoveVec.normalized * Time.deltaTime * moveSpeed;
            }
            else
            {
                cx = this.circleStandard.x + Mathf.Cos(circleTime);
                cy = this.circleStandard.y + Mathf.Sin(circleTime);

                this.circleTarget = new Vector3(cx, cy, 0);

                this.circleMoveVec = this.circleTarget - this.circleStandard;
            }
        }
        //보스패턴
        else if (this.enemyType == EnemyType.boss)
        {
            //일시정지
            if(GameManager.instance.pauseState)
            {
                this.moveSpeed = 0;
            }
            else
            {
                moveSpeed = 0.05f;
            }

            //보스는 계속 이동
            this.transform.position = Vector3.MoveTowards(this.transform.position,
                                                            player.transform.position,
                                                            moveSpeed);
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position,
                                                  player.transform.position,
                                                  moveSpeed * Time.deltaTime);
        }

        if (this.gameObject.activeInHierarchy)
        {
            distance = Vector2.Distance(this.transform.position, player.transform.position);
        }
    }

    //Setting Enemy Type
    public void SetType(EnemyType _type)
    {
        enemyType = _type;

        switch (enemyType)
        {
            case EnemyType.normal:
                this.renderer.gameObject.layer = 0;
                this.GetComponent<BoxCollider>().size = new Vector3(.25f, .25f, .5f);
                this.renderer.sprite = enemyImage[0];
                this.transform.localScale = new Vector3((GameManager.instance.stageNum * .25f) + 2,
                                                        (GameManager.instance.stageNum * .25f) + 2,
                                                        (GameManager.instance.stageNum * .25f) + 2);
                moveSpeed = 1.5f;
                break;
            case EnemyType.speeder:
                this.renderer.gameObject.layer = 0;
                this.GetComponent<BoxCollider>().size = new Vector3(.25f, .25f, .5f);
                this.renderer.sprite = enemyImage[1];
                this.transform.localScale = new Vector3((GameManager.instance.stageNum * .25f) + 2,
                                                        (GameManager.instance.stageNum * .25f) + 2,
                                                        (GameManager.instance.stageNum * .25f) + 2);
                moveSpeed = 2.5f;
                break;
            case EnemyType.tanker:
                this.renderer.gameObject.layer = 0;
                this.GetComponent<BoxCollider>().size = new Vector3(.25f, .25f, .5f);
                this.renderer.sprite = enemyImage[2];
                this.transform.localScale = new Vector3((GameManager.instance.stageNum * .25f) + 2,
                                                        (GameManager.instance.stageNum * .25f) + 2,
                                                        (GameManager.instance.stageNum * .25f) + 2);
                moveSpeed = 1.5f;
                break;
            case EnemyType.laser:
                this.renderer.gameObject.layer = 0;
                this.GetComponent<BoxCollider>().size = new Vector3(.25f, .25f, .5f);
                this.renderer.sprite = enemyImage[3];
                this.transform.localScale = new Vector3((GameManager.instance.stageNum * .25f) + 2,
                                                        (GameManager.instance.stageNum * .25f) + 2,
                                                        (GameManager.instance.stageNum * .25f) + 2);
                moveSpeed = 15f;

                if (GameManager.instance.stageNum >= 8)
                {
                    this.transform.localScale = new Vector3(3, 3, 3);
                }
                break;
            case EnemyType.circle:
                this.renderer.gameObject.layer = 0;
                this.GetComponent<BoxCollider>().size = new Vector3(.25f, .25f, .5f);
                this.renderer.sprite = enemyImage[3];
                this.transform.localScale = new Vector3((GameManager.instance.stageNum * .25f) + 2,
                                                        (GameManager.instance.stageNum * .25f) + 2,
                                                        (GameManager.instance.stageNum * .25f) + 2);
                moveSpeed = 7.5f;

                if (GameManager.instance.stageNum >= 8)
                {
                    this.transform.localScale = new Vector3(3, 3, 3);
                }
                break;
            case EnemyType.boss:
                this.renderer.gameObject.layer = 8;
                this.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, .5f);
                bossImageNum = Random.Range(0, 6);
                this.renderer.sprite = bossImage[bossImageNum];
                this.transform.localScale = new Vector3((GameManager.instance.stageNum  * .5f) + 2,
                                                        (GameManager.instance.stageNum * .5f) + 2,
                                                       (GameManager.instance.stageNum * .5f) + 2);
                moveSpeed = 0.05f;

                break;
        }
    }

    //Enemy죽음 체크
    public void DeadCheck()
    {
        if (Life <= 0)
        {
            //죽는 이펙트
            GameObject _effect = Instantiate(desEffect);
            _effect.transform.position = this.transform.position;
            Destroy(_effect, 0.5f);

            if (this.enemyType == EnemyType.boss)
            {
                _effect.transform.localScale = new Vector3(10, 10, 10);
                GameManager.instance.AddScore(10);      //10점

                //Item Drop
                for (int i = 0; i < 2; i++)
                {
                    GameObject dropBox = Instantiate(getBox);
                    dropBox.transform.position = player.transform.position;
                    if (i == 0)
                    {
                        dropBox.GetComponent<GetBox>().thisBoxType = GetBox.boxType.coin;
                    }
                    ItemManager.instance.boxItems.Add(dropBox);
                    dropBox.SetActive(true);
                }

                //Enemy전부 죽임
                foreach (GameObject _enemy in EnemyManager.instance.objList)
                {
                    if (_enemy.activeInHierarchy)
                    {
                        _enemy.SetActive(false);

                        _effect = Instantiate(desEffect);
                        _effect.transform.position = _enemy.transform.position;
                        Destroy(_effect, 0.5f);
                    }
                }

                SoundManager.instance.PlayEffectSound(2);

            }
            else if (this.enemyType == EnemyType.tanker)
            {
                GameManager.instance.AddScore(2);       //2점

                GameManager.instance.greenLoot++;

                SoundManager.instance.PlayEffectSound(1);
            }
            else if (this.enemyType == EnemyType.speeder)
            {
                GameManager.instance.AddScore(1);      //1점

                GameManager.instance.blueLoot++;

                SoundManager.instance.PlayEffectSound(1);
            }
            else
            {
                GameManager.instance.AddScore(1);      //1점

                GameManager.instance.redLoot++;

                SoundManager.instance.PlayEffectSound(1);
            }

            this.gameObject.SetActive(false);
        }
    }

    //Damage표시.
    private void ShowDamage(int _damage, Vector3 pos)
    {
        GameObject newlabel = Instantiate(damageLabel);

        newlabel.transform.localScale = new Vector3(1, 1, 1);
        newlabel.transform.position = pos;

        newlabel.transform.FindChild("label").GetComponent<UILabel>().text = _damage.ToString();

        newlabel.GetComponent<TweenPosition>().from = pos;
        newlabel.GetComponent<TweenPosition>().to = pos + (Vector3.up * 1f);

        Destroy(newlabel, 1f);
    }

    //충돌체크
    private void OnTriggerEnter(Collider other)
    {
        //총알에 닿으면
        if (other.transform.tag.Equals("Bullet"))
        {
            //이 오브젝트 타임이 직선운동이면 return
            if (this.enemyType == EnemyType.laser || this.enemyType == EnemyType.circle)
                return;

            //닿아도 사라지지 않는 총알들.
            if (BulletManager.instance.curBulletType != BulletManager.bulletType.laser
                && BulletManager.instance.curBulletType != BulletManager.bulletType.bounce
                && BulletManager.instance.curBulletType != BulletManager.bulletType.sword)
                other.gameObject.SetActive(false);

            //총알 데미지
            Life -= other.GetComponent<Bullet>().bulletDamage;

            //데미지 표시
            ShowDamage(other.GetComponent<Bullet>().bulletDamage, this.transform.position);

            DeadCheck();

            if (this.enemyType == EnemyType.boss)
            {
                EnemyManager.instance.bossCurLife = Life;
                EnemyManager.instance.bossTotalLife = standardLife;
            }
        }
        else if (other.transform.tag.Equals("Warning"))
        {
            if (this.enemyType == EnemyType.laser)
                other.gameObject.SetActive(false);
        }
        else if (other.transform.tag.Equals("BobmEffect"))
        {
            if (this.enemyType == EnemyType.laser)
                return;

            Life -= other.GetComponent<Bullet>().bulletDamage;
            
            //데미지 표시
            ShowDamage(other.GetComponent<Bullet>().bulletDamage, this.transform.position);

            DeadCheck();

            if (this.enemyType == EnemyType.boss)
            {
                EnemyManager.instance.bossCurLife = Life;
                EnemyManager.instance.bossTotalLife = standardLife;
            }
        }
        else if (other.transform.tag.Equals("Fever"))
        {
            Life = 0;

            ShowDamage(999, this.transform.position);

            DeadCheck();
        }
    }
}