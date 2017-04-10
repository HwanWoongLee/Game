using UnityEngine;
using System.Collections;

public class GameEndPop : MonoBehaviour {
	public UILabel topSocre, curScore;
	public GameObject madeBy;

	private void Update()
	{
		topSocre.text = "TOP SCORE\n" + GameManager.instance.gameData.topWave.ToString();
		curScore.text = "SCORE\n" + GameManager.instance.gameData.wave.ToString();
	}
	
	public void HomeButton()
	{
		PlayerPrefs.SetInt("FIRST",0);
        
		Application.LoadLevel("MainScene");
	}

	public void MadeByButton()
	{
		madeBy.GetComponent<TweenScale>().ResetToBeginning();
		madeBy.GetComponent<TweenScale>().Play();
		
		madeBy.SetActive(true);
	}
	public void MadeByOff()
	{
		madeBy.SetActive(false);
	}
	public void ShareButton()
	{
		GeneralSharingiOSBridge.ShareSimpleText("\nhttps://www.facebook.com/60Celsius/?fref=ts");
	}
}
