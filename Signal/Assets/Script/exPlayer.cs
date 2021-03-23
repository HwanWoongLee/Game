using UnityEngine;
using System.Collections;

public class exPlayer : MonoBehaviour
{
    Player _player;

    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float fireSpeed;
    [SerializeField]
    float damage;

    private float curTime = 0f;

    void Start()
    {
        _player = GameManager.instance.player;
        SetData();
    }

    private void SetData()
    {
        damage = _player.Damage;
        fireSpeed = _player.bulletDelayTime;
        moveSpeed = _player.moveSpeed;
    }

    private void PlayerFired()
    {
        curTime += Time.deltaTime;

        if (curTime >= fireSpeed)
        {
            BulletManager.instance.FireBullets(this.transform.position, Vector3.right, damage);

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

            curTime = 0f;
        }
    }

    void Update()
    {
        if (GameManager.instance.curGameState != GameState.store)
            return;

        SetData();
 
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);

        if (transform.position.x >= 10f)
        {
            transform.localPosition = new Vector3(-5500f, 500f, 0f);
        }

        PlayerFired();
    }

}
