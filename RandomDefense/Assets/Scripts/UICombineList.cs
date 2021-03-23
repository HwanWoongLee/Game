using UnityEngine;
using System.Collections;

public class UICombineList : MonoBehaviour {
    public UILabel baseLabel;
    public UILabel matLabel1;
    public UILabel matLabel2;
    public UILabel resultLabel;

    public int baseID   { get; set; }
    public int matID01  { get; set; }
    public int matID02  { get; set; }
    public int resultID { get; set; }

    public void RendLabel(CombineManager _combine,int index)
    {
        baseID      = _combine.baseID[index];
        matID01     = _combine.matID_01[index];
        matID02     = _combine.matID_02[index];
        resultID    = _combine.resultID[index];

        baseLabel.text      = baseID.ToString();
        matLabel1.text      = matID01.ToString();
        resultLabel.text    = resultID.ToString();

        if (matID02 != 0)
            matLabel2.text = matID02.ToString();
    }

}
