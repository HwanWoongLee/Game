using UnityEngine;
using System.Collections;

public class FirstCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {

		if (!DataManager.Instance.FirstDecide())
        {
			Application.LoadLevel("GameScene");
		}
	}
}
