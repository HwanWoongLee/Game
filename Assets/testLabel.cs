using UnityEngine;
using System.Collections;

public class testLabel : MonoBehaviour {

    UILabel thisLabel;

    private void Start()
    {
        thisLabel = GetComponent<UILabel>();
    }

    // Update is called once per frame
    void Update () {
        thisLabel.text = "Damage : " + GameManager.instance.player.Damage.ToString() + "\n"
                            + "FireSpeed : " + GameManager.instance.player.bulletDelayTime.ToString() + "\n"
                             + "MoveSpeed : " + GameManager.instance.player.moveSpeed.ToString();
	}
}
