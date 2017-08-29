using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
    public static SoundManager instance;

    public AudioClip[] effecSound;
    public AudioClip bgmSound;

    private AudioSource effectAudio;

    public AudioSource bgmAudio;

    //Sound On,Off
    public int SoundState = 0;
    public GameObject soundButton;
    public GameObject muteButton;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        effectAudio = this.GetComponent<AudioSource>();

        SoundState = PlayerPrefs.GetInt("SOUND");

        if(SoundState == 0)
        {
            SoundOn();
        }
        else
        {
            SoundOff();
        }
    }

    public void PlayEffectSound(int _num)
    {
        effectAudio.PlayOneShot(effecSound[_num]);
    }
 
    public void PlayBGMSound()
    {
        bgmAudio.Play();
    }

    public void StopBGMSound()
    {
        bgmAudio.Stop();
    }

    public void SoundOn()
    {
        soundButton.SetActive(true);
        muteButton.SetActive(false);

        this.GetComponent<AudioSource>().mute = false;
        this.bgmAudio.GetComponent<AudioSource>().mute = false;

        SoundState = 0;
        PlayerPrefs.SetInt("SOUND", SoundState);
    }

    public void SoundOff()
    {
        soundButton.SetActive(false);
        muteButton.SetActive(true);

        this.GetComponent<AudioSource>().mute = true;
        this.bgmAudio.GetComponent<AudioSource>().mute = true;

        SoundState = 1;
        PlayerPrefs.SetInt("SOUND", SoundState);
    }
}
