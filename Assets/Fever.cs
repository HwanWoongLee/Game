using UnityEngine;
using System.Collections;

public class Fever : ObjectManager
{
    private float curTime = 0f;
    private float fireTime = 0f;
    private float endTime = 10f;

    private float fx, fy;

    private Player _player;
    public GameObject feverBullet;

    private Vector3 moveVec;
    private Vector3 targetVec;

    public bool fever;

    private void Start()
    {
        MakeObjs(feverBullet);

        _player = GameManager.instance.player;
        fireTime = 0f;

        fever = false;
    }


    void FixedUpdate()
    {
        if (fever)
        {
            curTime += Time.deltaTime * 7f;

            fx = _player.transform.position.x + Mathf.Cos(curTime);
            fy = _player.transform.position.y + Mathf.Sin(curTime);

            targetVec = new Vector3(fx, fy);

            moveVec = targetVec - _player.transform.position;

            if (curTime >= fireTime)
            {
                GameObject _bullet = GetObj();
                _bullet.transform.position = _player.transform.position;
                _bullet.GetComponent<FeverBullet>().fireVec = moveVec.normalized;
                _bullet.SetActive(true);

                fireTime += .2f;
            }

            if(curTime >= endTime)
            {
                curTime = 0f;
                fireTime = 0f;
                fever = false;
            }
        }
    }
}
