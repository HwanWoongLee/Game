using UnityEngine;
using System.Collections;

public class Homing : MonoBehaviour
{
    Transform myTransform;

    [SerializeField]
    Transform TargetTransform;

    Vector3 Dir;
    Vector3 Axis;

    Quaternion newRotation;

    public float homingDelay = 0;
    float destroyTime = 0;
    int num = 0;

    private void Start()
    {
        myTransform = this.transform;
    }

    private void OnEnable()
    {
        homingDelay = 0;
        FindEnemy();
        num++;
    }
    private void OnDisable()
    {
        TargetTransform = null;
        homingDelay = 0;
    }

    void Update()
    {
        destroyTime += Time.deltaTime;
        homingDelay += Time.deltaTime;

        if (destroyTime >= 8f)
        {
            this.gameObject.SetActive(false);
            destroyTime = 0f;
        }
        if (homingDelay >= .4f)
        {
            if (TargetTransform != null)
            {
                Dir = myTransform.position - TargetTransform.position;
                Axis = Vector3.Cross(Dir, myTransform.up);

                newRotation = Quaternion.AngleAxis(Time.deltaTime * 360, Axis) * myTransform.rotation;

                myTransform.rotation = Quaternion.Lerp(myTransform.rotation, newRotation, 50 * Time.deltaTime);
            }

            this.transform.Translate(Vector3.up * Time.deltaTime * 10f);
        }
    }

    //유도미사일, 적찾기
    public void FindEnemy()
    {
        if (num == 0)
            return;

        float min = 10;
        int findNum = 0;
        bool state = false;

        for (int i = 0; i < EnemyManager.instance.objList.Count; i++)
        {
            if (EnemyManager.instance.objList[i].activeInHierarchy)
            {
                //직선패턴은 제외
                if (EnemyManager.instance.objList[i].GetComponent<Enemy>().enemyType != EnemyType.laser)
                {
                    float _distance = EnemyManager.instance.objList[i].GetComponent<Enemy>().distance;

                    if (_distance <= min)
                    {
                        min = _distance;
                        findNum = i;
                    }
                }
            }
            if (i == EnemyManager.instance.objList.Count - 1)
            {
                state = true;
            }
        }

        if (state)
        {
            if (EnemyManager.instance.objList[findNum].activeInHierarchy)
            {
                TargetTransform = EnemyManager.instance.objList[findNum].transform;
            }
            else
                TargetTransform = null;
        }
    }
}
