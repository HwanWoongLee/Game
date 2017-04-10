using UnityEngine;
using System.Collections;

public class LogoCam : MonoBehaviour {
	Animator myAni;
	float dTime;

	// Use this for initialization
	void Start () {
		dTime = 0f;
		myAni = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		dTime += Time.deltaTime;
		
		if(dTime >= 4f)
		{
			myAni.enabled = true;
		}
	}
}
