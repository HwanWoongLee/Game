using UnityEngine;
using System.Collections;

public class ByePop : MonoBehaviour {

	public void Yes()
	{
		Application.Quit();
	}

	public void No()
	{
		GameManager.instance.byePopState = false;
		this.gameObject.SetActive(false);
	}
}
