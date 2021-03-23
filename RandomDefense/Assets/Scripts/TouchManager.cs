using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour
{
    private int targetUnitID;
    private GameObject targetUnit;

    bool targetOn;
    CombineManager myCombineManager;

    private void Start()
    {
        targetOn = false;
        myCombineManager = transform.GetComponent<CombineManager>();
    }

    void ClickUnit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //유닛 클릭시
                if (hit.transform.CompareTag("Unit"))
                {
                    targetUnit = hit.transform.gameObject;
                    targetOn = true;

                    //유닛 id값을 가져옴.
                    GetUnitID(targetUnit);

                    UIManager.instance.SendUnitInfo(targetUnitID);
                }
                else if (hit.transform.CompareTag("CombineList"))
                {
                    ClickCombineList(hit);
                    targetOn = false;
                }
                else if (hit.transform.CompareTag("Tile"))
                {
                    //유닛 선택중에
                    if (targetOn)
                    {
                        //해당 유닛타일 Type을 normal로바꾸고
                        GameManager.instance.myMapGenerator.TileUnitOff(targetUnit.transform.position);

                        //클릭한 타일Pos(를 가져옴과 동시에 unitTile로 바꿈)
                        targetUnit.transform.position =
                            GameManager.instance.myMapGenerator.GetTilePos(hit.transform.position);

                        targetOn = false;
                        UIManager.instance.OffUnitInfo();
                    }
                }
                else
                {
                    targetOn = false;
                    UIManager.instance.OffUnitInfo();
                }
            }

        }
    }
    void ClickCombineList(RaycastHit hit)
    {
        UICombineList targetList;
        targetList = hit.transform.GetComponent<UICombineList>();

        //유닛조합
        myCombineManager.CombineUnit(targetUnit,
            targetList.matID01,
            targetList.matID02,
            targetList.resultID);
    }

    void GetUnitID(GameObject _unit)
    {
        targetUnitID = _unit.GetComponent<Unit>().id;
    }

    private void Update()
    {
        ClickUnit();
    }
}
