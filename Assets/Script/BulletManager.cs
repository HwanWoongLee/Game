using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager : ObjectManager
{
    public static BulletManager instance;

    public enum bulletType
    {
        normal,
        big,
        laser,
        bounce,
        guided,
        sword,
        explosion       //7가지
    }

    public bulletType curBulletType = bulletType.normal;

    //총알들을 배열리스트로 저장
    public List<GameObject[]> _objList = new List<GameObject[]>();

    public int typeNum = 0;
    public float bulletScale;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < makeObj.Length; i++)
        {
            this.MakeObjs(this.makeObj[i]);
        }
    }

    public override void MakeObjs(GameObject _obj)
    {
        GameObject[] tempArray = new GameObject[maxNum];

        for (int i = 0; i < maxNum; i++)
        {
            GameObject newObj = Instantiate(_obj);
            newObj.transform.parent = this.transform;
            newObj.SetActive(false);
            tempArray[i] = newObj;

            if (i == maxNum - 1)
            {
                this._objList.Add(tempArray);
            }
        }
    }

    public override GameObject GetObj()
    {
        foreach (GameObject _obj in _objList[typeNum])
        {
            if (!_obj.activeInHierarchy)
            {
                return _obj;
            }
        }
        return null;
    }

    public override void InitObjs()
    {
        foreach(GameObject[] _objs in _objList)
        {
            for(int i=0;i<_objs.Length;i++)
            {
                _objs[i].SetActive(false);
            }
        }
    }

    //총알 발사.
    public void FireBullets(Vector3 _pos)
    {
        //총알하나 불러와서
        GameObject fireBullet = GetObj();
        fireBullet.transform.position = _pos + (player.fireDirection * .5f);
        fireBullet.transform.localScale = new Vector3(bulletScale, bulletScale, bulletScale);
        fireBullet.SetActive(true);

        //발사
        fireBullet.GetComponent<Bullet>().SetFireDirection(player.fireDirection);
     
        if (fireBullet.GetComponent<TweenRotation>())
        {
            fireBullet.GetComponent<TweenRotation>().ResetToBeginning();
            fireBullet.GetComponent<TweenRotation>().Play();
        }
        fireBullet.GetComponent<Bullet>().SetDamage(player.Damage);
        fireBullet.GetComponent<Bullet>().isFire = true;
    }

    public void FireBullets(Vector3 _pos, Vector3 _dir, float _damage)
    {
        //총알하나 불러와서
        GameObject fireBullet = GetObj();
        fireBullet.transform.position = _pos;
        fireBullet.transform.localScale = new Vector3(bulletScale,bulletScale,bulletScale);
        fireBullet.SetActive(true);

        //발사
        fireBullet.GetComponent<Bullet>().SetFireDirection(_dir);

        if (fireBullet.GetComponent<TweenRotation>())
        {
            fireBullet.GetComponent<TweenRotation>().ResetToBeginning();
            fireBullet.GetComponent<TweenRotation>().Play();
        }
        fireBullet.GetComponent<Bullet>().SetDamage((int)_damage);
        fireBullet.GetComponent<Bullet>().isFire = true;
    }

    //Bullet Type
    public void SetBulletType(int _typeNum)
    {
        this.typeNum = _typeNum;

        switch (_typeNum)
        {
            case 0:
                this.curBulletType = bulletType.normal;
                break;
            case 1:
                this.curBulletType = bulletType.big;
                break;
            case 2:
                this.curBulletType = bulletType.laser;
                break;
            case 3:
                this.curBulletType = bulletType.bounce;
                break;
            case 4:
                this.curBulletType = bulletType.guided;
                break;
            case 5:
                this.curBulletType = bulletType.sword;
                break;
            case 6:
                this.curBulletType = bulletType.explosion;
                break;
        }
        
    }
}
