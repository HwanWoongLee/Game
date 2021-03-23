using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {
    public static Fade instance;

    public enum FadeState
    {
        _in,
        _out,
        _none
    }

    public FadeState curFadeState = FadeState._none;
    
    Color fadeColor;        //fade색.

    float fadeTime;         //기준시간.
    float fTime;            //흐르는시간.

    UISprite image;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start () {
        fadeColor = new Color(0f, 0f, 0f, 0f);
        fadeTime = .5f;
        fTime = 0f;
        image = this.GetComponent<UISprite>();
    }
	
	void Update () {
        if (curFadeState == FadeState._in)
            StartCoroutine(FadeIn());
        else if (curFadeState == FadeState._out)
            StartCoroutine(FadeOut());
		else
		{
			fadeColor = new Color(0f, 0f, 0f, 0f);
			image.color = fadeColor;
		}

    }

    public void FadeInOut()
    {
        this.curFadeState = FadeState._out;
    }

    public IEnumerator FadeOut()
    {
        fTime += Time.deltaTime;

        if (fTime <= fadeTime)
        {
            yield return new WaitForEndOfFrame();

            fadeColor.a = Mathf.Clamp01(fTime / fadeTime);
            image.color = fadeColor;
        }
        else
        {
            curFadeState = FadeState._in;
            fTime = 0f;
        }
    }

    public IEnumerator FadeIn()
    {
        fTime += Time.deltaTime;

        if (fTime <= fadeTime)
        {
            yield return new WaitForEndOfFrame();

            fadeColor.a = 1.0f - Mathf.Clamp01(fTime / fadeTime);
            image.color = fadeColor;
        }
        else
        {
            curFadeState = FadeState._none;
            fTime = 0f;
        }
    }

}