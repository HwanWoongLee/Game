using UnityEngine;
using System.Collections;

public class UIUnitInfo : MonoBehaviour
{
    public UILabel name;

    public CombineManager combine;
    public GameObject[] combineList;
    public GameObject[] combineListLong;

    public void SetUnitInfo(int _id)
    {
        for(int i = 0; i < combineList.Length; i++)
        {
            combineList[i].SetActive(false);
            combineListLong[i].SetActive(false);
        }

        name.text = _id.ToString();

        //해당 id로 matid를 찾아낸다.
        combine.FIndMatID(_id);

        //조합의 개수만큼 조합리스트UI 활성화
        for (int i = 0; i < combine.baseID.Count; i++)
        {
            //2번째 조합재료가 있을시
            if (combine.matID_02[i] != 0)
            {
                //긴녀석을 활성화.
                combineListLong[i].GetComponent<UICombineList>().RendLabel(combine, i);
                combineListLong[i].SetActive(true);
            }
            else
            {
                combineList[i].GetComponent<UICombineList>().RendLabel(combine, i);
                combineList[i].SetActive(true);
            }
        }
    }
}