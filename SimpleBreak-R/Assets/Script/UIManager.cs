using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public UILabel curScore,
                    topScore,
                    coinCount,
                    coinCount2;

    bool playbutton = false;
    float dTime = 0f;

    void Start()
    {
        if (Fade.instance != null)
            Fade.instance.curFadeState = Fade.FadeState._in;
    }

    void Update()
    {
        if (curScore != null)
            curScore.text = GameManager.instance.gameData.wave.ToString();
        if (topScore != null)
            topScore.text = "TOP\n" + GameManager.instance.gameData.topWave.ToString();
        if (coinCount != null)
            coinCount.text = GameManager.instance.gameData.coin.ToString();
        if (coinCount2 != null)
            coinCount2.text = GameManager.instance.gameData.coin.ToString();
                

        //GameStart
        if (playbutton)
        {
            dTime += Time.deltaTime;

            if (dTime >= .5f)
            {
                dTime = 0f;
                playbutton = false;
                if (PlayerPrefs.GetInt("TUTORIAL") == 0)
                {
					Application.LoadLevel("TutorialScene");
                }
                else if (PlayerPrefs.GetInt("TUTORIAL") == 1)
                {
                    Application.LoadLevel("GameScene");
                }
            }
        }
    }

    public void MainPlayButton()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.PlayEffectSound(0);

        Fade.instance.curFadeState = Fade.FadeState._out;
        playbutton = true;
    }

    public void FaceBookButton()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.PlayEffectSound(0);

        Application.OpenURL("https://www.facebook.com/60Celsius");
    }
}
