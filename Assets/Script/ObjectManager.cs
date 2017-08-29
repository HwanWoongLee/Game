using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour
{
    //생성할 프리팹 오브젝트
    public GameObject[] makeObj;

    public Player player;

    //오브젝트 리스트
    public List<GameObject> objList = new List<GameObject>();

    //오브젝트 최대개수
    public int maxNum = 200;

    //생성
    public virtual void MakeObjs(GameObject _obj)
    {
        for (int i = 0; i < maxNum; i++)
        {
            GameObject newObj = Instantiate(_obj);
            newObj.transform.parent = this.transform;
            newObj.SetActive(false);

            objList.Add(newObj);
        }
    }

    //오브젝트 가져오기
    public virtual GameObject GetObj()
    {
        foreach (GameObject _obj in objList)
        {
            if (!_obj.activeInHierarchy)
            {
                return _obj;
            }
        }
        return null;
    }

    //오브젝트 전체 초기화
    public virtual void InitObjs()
    {
        foreach(GameObject _obj in objList)
        {
            _obj.SetActive(false);
        }
    }
}
