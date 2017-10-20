using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class CombineManager : MonoBehaviour
{
    [Serializable]
    class CombineData
    {
        public int baseID;
        public int matID01;
        public int matID02;
        public int resultID;
    }
    CombineData[] myData = null;
    TextAsset dataPath;

    public List<int> baseID     = new List<int>();
    public List<int> matID_01   = new List<int>();
    public List<int> matID_02   = new List<int>();
    public List<int> resultID   = new List<int>();

    public void CombineUnit(GameObject _baseUnit,int _matID01,int _matID02,int _resultID)
    {
        List<GameObject> combineUnitList = new List<GameObject>();

        GameObject baseUnit = _baseUnit;
        GameObject mat01Unit = FindCombineUnit(_matID01,baseUnit);

        combineUnitList.Add(baseUnit);
        combineUnitList.Add(mat01Unit);
        if (_matID02 != 0)
        {
            GameObject mat02Unit = FindCombineUnit(_matID02,baseUnit,mat01Unit);
            combineUnitList.Add(mat02Unit);
        }

        foreach (GameObject _unit in combineUnitList)
        {
            //유닛 리스트중 null이 있는경우
            if (_unit == null)
            {
                Debug.Log("유닛부족!조합실패!");
                return;
            }
        }

        //유닛이 모두 있을경우
        foreach (GameObject _unit in combineUnitList)
        {
            //유닛이 있는 타일타입을 바꿔줌
            GameManager.instance.myMapGenerator.TileUnitOff(_unit.transform.position);
            //재료유닛 비활성화
            _unit.SetActive(false);

            UnitManager.instance.curUnitNum--;
            UIManager.instance.OffUnitInfo();
        }

        //결과유닛 생성
        UnitManager.instance.SpawnUnit(
            GameManager.instance.myMapGenerator.GetTilePos(_baseUnit.transform.position),
            _resultID);
    }

    //조합리스트에 있는 유닛이 있는지 검사.
    private GameObject FindCombineUnit(int _findID, GameObject _baseUnit)
    {
        foreach(GameObject _unit in UnitManager.instance.unitList)
        {
            //찾는 유닛이 base유닛이랑 중복되지 않게 처리.
            if(_unit == _baseUnit)
            {
                continue;
            }

            //활성화 되어있는 유닛중
            if (_unit.activeInHierarchy)
            {
                //찾는 ID값과 같은 ID가 있을경우.
                if (_unit.GetComponent<Unit>().id == _findID)
                {
                    return _unit;
                }
                //찾는 ID가 없으면 다음검색
                else
                    continue;
            }
        }
        //없을경우 null값 반환
        return null;
    }
    private GameObject FindCombineUnit(int _findID, GameObject _baseUnit, GameObject _mat01Unit)
    {
        foreach (GameObject _unit in UnitManager.instance.unitList)
        {
            //찾는 유닛이 base유닛이랑 중복되지 않게 처리.
            if (_unit == _baseUnit || _unit == _mat01Unit)
            {
                continue;
            }

            //활성화 되어있는 유닛중
            if (_unit.activeInHierarchy)
            {
                //찾는 ID값과 같은 ID가 있을경우.
                if (_unit.GetComponent<Unit>().id == _findID)
                {
                    return _unit;
                }
                //찾는 ID가 없으면 다음검색
                else
                    continue;
            }
        }
        //없을경우 null값 반환
        return null;
    }

    //데이터 경로 설정
    private void SetDataPath()
    {
        dataPath = Resources.Load("combine1") as TextAsset;
    }

    //재료찾기
    public void FIndMatID(int _baseID)
    {
        SetDataPath();
        LoadAndCompareData(_baseID);
    }

    //데이터로드 & 비교
    private void LoadAndCompareData(int _baseID)
    {
        string json;

        myData = null;

        //dataPath의 문자열을 저장.
        json = dataPath.ToString();

        //json형식의 문자열을 mydata변수에 저장.
        myData = JsonHelper.FromJson<CombineData>(json);
           
        //id비교
        CompareID(_baseID);
    }

    //비교
    private void CompareID(int _baseID)
    {
        //ID리스트 비움.
        baseID.Clear();
        matID_01.Clear();
        matID_02.Clear();
        resultID.Clear();

        for (int i = 0; i < myData.Length; i++)
        {
            //baseID값이 일치할 때
            if (_baseID == myData[i].baseID)
            {
                //재료유닛ID에 ID값 저장.
                baseID.Add(myData[i].baseID);
                matID_01.Add( myData[i].matID01);
                matID_02.Add( myData[i].matID02);
                resultID.Add(myData[i].resultID);
            }
        }
    }
}
