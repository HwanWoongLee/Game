using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public static SoundManager instance;

	public AudioClip[] effectSound;

	private AudioSource myAudio;

	void Awake(){
		if(instance == null)
			instance = this;
		
		myAudio = this.GetComponent<AudioSource>();

		if(PlayerPrefs.GetInt("MUTE") == 1)
		{
			myAudio.mute = true;
		}
		else if(PlayerPrefs.GetInt("MUTE") == 0)
		{
			myAudio.mute = false;
		}
	}
	
	public void PlayEffectSound(int num)
	{
		if(num == 8){
			myAudio.Stop();
		}
		myAudio.PlayOneShot(effectSound[num]);
	}
}
