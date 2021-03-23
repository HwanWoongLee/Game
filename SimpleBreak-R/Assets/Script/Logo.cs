using UnityEngine;
using System.Collections;

public class Logo : MonoBehaviour {
	float dTime = 0f;
	
	private AudioSource myAudio;

	void Start()
	{
		myAudio = this.GetComponent<AudioSource>();
		Time.timeScale = 1f;

		if(PlayerPrefs.GetInt("MUTE") == 1)
		{
			myAudio.mute = true;
		}
		else if(PlayerPrefs.GetInt("MUTE") == 0)
		{
			myAudio.mute = false;
		}
	}

	// Update is called once per frame
	void Update () {
		dTime += Time.deltaTime;
		
		if(dTime >= 4f)
		{
			myAudio.enabled = true;
		}
		if(dTime >= 7f)
			Application.LoadLevel("MainScene");
	}
}
