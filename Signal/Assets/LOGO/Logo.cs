using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Logo : MonoBehaviour {
    [SerializeField]
    public AudioSource myAudio;
    public Animator myAni;

    public bool isAnimationEnd = false;
    void Awake()
    {
        #if UNITY_IOS
        Application.targetFrameRate = 60;
        #endif
    }
	IEnumerator Start()
	{
		while (Application.isShowingSplashScreen)
		{
			yield return null;
		}

		myAudio.enabled = true;
		myAni.enabled = true;

		yield return null;
	}
    void Update()
    {
        if (isAnimationEnd)
        {
            SceneManager.LoadScene("Game");
            isAnimationEnd = false;
        }
    }
    public void OnAnimationEnd()
    {
        isAnimationEnd = true;
    }
}