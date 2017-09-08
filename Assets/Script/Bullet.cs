using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public bool isFire = false;

    [SerializeField]
    private Vector2 fireDirection = Vector2.up;

    //총알속도
    private float bulletSpeed = 15f;
    private float laserWidth = 1f;

    public BulletManager.bulletType thisType = BulletManager.bulletType.normal;

    [SerializeField]
    private GameObject[] exEffect;

    public int bulletDamage;
    private int bounceNum = 3;

    private void OnEnable()
    {
        thisType = BulletManager.instance.curBulletType;
        if (thisType == null)
            thisType = BulletManager.bulletType.normal;

        bounceNum = 3;

        if (thisType == BulletManager.bulletType.laser)
        {
            laserWidth = BulletManager.instance.bulletScale;
            this.transform.localScale = new Vector3(laserWidth, laserWidth * 10f, 1f);
        }
    }

    void Update()
    {
        if (GameManager.instance.curGameState != GameState.game
            && GameManager.instance.curGameState != GameState.store
            && GameManager.instance.curGameState != GameState.store2)
            return;

        if (isFire)
        {
            FireBullet();

            //거리가 15이상이면 꺼줌
            if (DistanceToPlayer() >= 15f)
            {
                isFire = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    //발사
    private void FireBullet()
    {
        if (thisType == BulletManager.bulletType.laser)
        {
            laserWidth -= Time.deltaTime * 5f * BulletManager.instance.bulletScale;
            this.transform.localScale = new Vector3(laserWidth, 20f, 1f);

            if (laserWidth <= 0f)
            {
                this.gameObject.SetActive(false);
                laserWidth = 1f;
            }
        }
        else if (thisType == BulletManager.bulletType.guided)
        {
            if (this.GetComponent<Homing>().homingDelay < .4f)
            {
                this.transform.Translate(Vector3.up * Time.deltaTime * bulletSpeed, Space.Self);
            }
            else
            {
                //homing.cs
            }
        }
        else if (thisType == BulletManager.bulletType.sword)
        {
            this.transform.Translate(Vector3.up * Time.deltaTime * bulletSpeed, Space.Self);
        }
        else if (thisType == BulletManager.bulletType.explosion)
        {
            this.transform.Translate(fireDirection * Time.deltaTime * bulletSpeed, Space.Self);
        }
        else if (thisType == BulletManager.bulletType.big)
        {
            this.transform.Translate(Vector3.up * Time.deltaTime * bulletSpeed, Space.Self);
        }
        else
        {
            this.transform.Translate(fireDirection * Time.deltaTime * bulletSpeed, Space.Self);
        }
    }

    //플레리어와 거리
    private float DistanceToPlayer()
    {
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = this.transform.position;

        return Vector3.Distance(myPos, playerPos);
    }

    //총알 발사방향
    public void SetFireDirection(Vector3 dir)
    {
        if (thisType == BulletManager.bulletType.laser)
        {
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

            this.transform.rotation = Quaternion.Euler(0, 0, -angle);

        }
        else if (thisType == BulletManager.bulletType.sword)
        {
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

            this.GetComponent<TweenRotation>().from = new Vector3(0, 0, -angle);
            this.GetComponent<TweenRotation>().to = new Vector3(0, 0, -angle - 180f);
        }
        else if (thisType == BulletManager.bulletType.big || thisType == BulletManager.bulletType.guided)
        {
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

            this.transform.rotation = Quaternion.Euler(0, 0, -angle);
        }

        this.fireDirection = dir;

    }


    //총알 데미지 설정
    public void SetDamage(int _damage)
    {
        bulletDamage = _damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Enemy"))
        {
            if (GameManager.instance.curGameState == GameState.game)
            {
                if (other.transform.GetComponent<Enemy>().enemyType == EnemyType.circle
                    || other.transform.GetComponent<Enemy>().enemyType == EnemyType.laser)
                {
                    return;
                }
            }

            if (this.thisType == BulletManager.bulletType.bounce)
            {
                bounceNum--;

                //팅기는 방향
                int _randNum = Random.Range(1, 9);
                Vector3 randPos = Vector3.up;

                switch (_randNum)
                {
                    case 1:
                        randPos = Vector3.up;
                        break;
                    case 2:
                        randPos = Vector3.up + Vector3.right;
                        break;
                    case 3:
                        randPos = Vector3.right;
                        break;
                    case 4:
                        randPos = Vector3.right + Vector3.down;
                        break;
                    case 5:
                        randPos = Vector3.down;
                        break;
                    case 6:
                        randPos = Vector3.down + Vector3.left;
                        break;
                    case 7:
                        randPos = Vector3.left;
                        break;
                    case 8:
                        randPos = Vector3.left + Vector3.up;
                        break;

                }

                SetFireDirection(randPos.normalized);

                //더이상 팅길수 없을때 Off
                if (bounceNum <= 0)
                {
                    OffBullet();
                }

                //피격이펙트
                GameObject _effect = BulletManager.instance.GetEffect(9);
                _effect.transform.position = other.transform.position;
                _effect.GetComponent<ParticleSystem>().time = 0;
                _effect.GetComponent<ParticleSystem>().Play();
                _effect.gameObject.SetActive(true);               
            }
            else if (this.thisType == BulletManager.bulletType.sword)
            {
                GameObject _effect = BulletManager.instance.GetEffect(9);
                _effect.transform.position = other.transform.position;
                _effect.GetComponent<ParticleSystem>().time = 0;
                _effect.GetComponent<ParticleSystem>().Play();
                _effect.gameObject.SetActive(true);            
            }
            else if (this.thisType == BulletManager.bulletType.explosion)
            {
                GameObject _effect = BulletManager.instance.GetEffect(8);
                _effect.transform.position = other.transform.position;
                _effect.GetComponent<ParticleSystem>().time = 0;
                _effect.GetComponent<ParticleSystem>().Play();

                _effect.transform.localScale = new Vector3(BulletManager.instance.bulletScale,
                                                           BulletManager.instance.bulletScale,
                                                           BulletManager.instance.bulletScale);

                _effect.gameObject.SetActive(true);
            }
            else
            {
                GameObject _effect = BulletManager.instance.GetEffect(7);
                _effect.transform.position = other.transform.position;
                _effect.GetComponent<ParticleSystem>().time = 0;
                _effect.GetComponent<ParticleSystem>().Play();
                _effect.gameObject.SetActive(true);            
            }
        }

    }

    public void OffBullet()
    {
        this.gameObject.SetActive(false);
    }
}
