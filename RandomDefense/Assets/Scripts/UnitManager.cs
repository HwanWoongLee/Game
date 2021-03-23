using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour {
    static public UnitManager instance;

    public GameObject unitPrefab;

    public List<GameObject> unitList = new List<GameObject>();

    public int unitMaxNum { get; set; }     //유닛 최대치
    public int curUnitNum { get; set; }     //현재 유닛수
    
  
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void MakeUnit()
    {
        if (transform.FindChild("unitHolder"))
        {
            unitList.Clear();

            Destroy(transform.FindChild("unitHolder").gameObject);
        }

        Transform unitHolder = new GameObject("unitHolder").transform;
        unitHolder.parent = this.transform;

        unitMaxNum = 50;

        for(int i = 0; i < unitMaxNum; i++)
        {
            GameObject newUnit = Instantiate(unitPrefab);
            newUnit.transform.parent = unitHolder;
            newUnit.SetActive(false);

            unitList.Add((newUnit));
        }
    }

    private GameObject GetUnit()
    {
        foreach (GameObject unit in unitList)
        {
            if (!unit.activeInHierarchy)
                return unit;
        }

        Debug.LogError("no unit in list!");
        return null;
    }

    public void SpawnUnit(Vector3 _pos)
    {
        GameObject newUnit = GetUnit();
        newUnit.transform.position = _pos;
        newUnit.SetActive(true);
        newUnit.GetComponent<Unit>().SetUnit(Random.Range(1,6));

        curUnitNum++;
    }
    public void SpawnUnit(Vector3 _pos, int _id)
    {
        GameObject newUnit = GetUnit();
        newUnit.transform.position = _pos;
        newUnit.SetActive(true);
        newUnit.GetComponent<Unit>().SetUnit(_id);

        curUnitNum++;
    }
}