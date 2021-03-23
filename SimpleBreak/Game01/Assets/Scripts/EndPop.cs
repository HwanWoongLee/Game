using UnityEngine;
using System.Collections;

public class EndPop : MonoBehaviour {
    public UILabel topScore;
    public UILabel score;
    public GameObject made;
    public GameObject backButton;

	void Update () {
        topScore.text = "TOP SCORE\n" + GameManager.instance.topWave.ToString();
        score.text = "SCORE\n" + GameManager.instance.curWave.ToString();
	}

    public void ClickHomeButton()
    {
        SoundManager.instance.PlaySound(0);

        DataManager.Instance.term = 0;

        DataManager.Instance.SaveData();
        DataManager.Instance.InitData();
 
        Application.LoadLevel("MainScene");
    }
    public void ClickShareButton()
    {
        GeneralShare generalShare = new GeneralShare();

        generalShare.shareText("", " Can You Do This?\nTop Score : " + GameManager.instance.topWave + "\n https://play.google.com/store/apps/details?id=com.SIXTYC.PB");
        
    }
    public void MadeByButton()
    {
        SoundManager.instance.PlaySound(0);

        made.SetActive(true);
        backButton.SetActive(true);
    }
    public void BackButton()
    {
        SoundManager.instance.PlaySound(0);

        made.SetActive(false);
        backButton.SetActive(false);
    }
}
