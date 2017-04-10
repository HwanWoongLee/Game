using UnityEngine;
using System.Collections;
//using UnityEngine.Advertisements;

public class GameOverPop : MonoBehaviour
{
    public GameObject mute1, mute2, helpPop;

    void Start()
    {
        if(mute1 == null)
            return;
            
        if (PlayerPrefs.GetInt("MUTE") == 1)
        {
            mute1.SetActive(false);
            mute2.SetActive(true);
            SoundManager.instance.GetComponent<AudioSource>().mute = true;
        }
        else if (PlayerPrefs.GetInt("MUTE") == 0)
        {
            mute1.SetActive(true);
            mute2.SetActive(false);
            SoundManager.instance.GetComponent<AudioSource>().mute = false;
        }
    }

    public void OneMoreAdsButton()
    {
        SoundManager.instance.PlayEffectSound(0);

        //광고보여주고 한번더 게임
        //ShowRewardedAd();
    }

    public void HelpButton()
    {
        helpPop.GetComponent<TweenScale>().ResetToBeginning();
        helpPop.GetComponent<TweenScale>().Play();
        helpPop.SetActive(true);
    }
    public void HelpButtonOff()
    {
        SoundManager.instance.PlayEffectSound(0);

        helpPop.SetActive(false);
    }
    public void GoHomeButton()
    {
        SoundManager.instance.PlayEffectSound(0);

        PlayerPrefs.SetInt("FIRST", 0);

        Application.LoadLevel("MainScene");
    }

    public void SoundOffButton()
    {
        mute1.SetActive(false);
        mute2.SetActive(true);

        SoundManager.instance.GetComponent<AudioSource>().mute = true;

        PlayerPrefs.SetInt("MUTE", 1);
    }

    public void SoundOnButton()
    {
        mute1.SetActive(true);
        mute2.SetActive(false);

        SoundManager.instance.GetComponent<AudioSource>().mute = false;

        PlayerPrefs.SetInt("MUTE", 0);
    }

    // public void ShowRewardedAd()
    // {
    //     if (Advertisement.IsReady("rewardedVideo"))
    //     {
    //         var options = new ShowOptions { resultCallback = HandleShowResult };
    //         Advertisement.Show("rewardedVideo", options);
    //     }
    // }

    // private void HandleShowResult(ShowResult result)
    // {
    //     switch (result)
    //     {
    //         case ShowResult.Finished:
    //             Debug.Log("The ad was successfully shown.");
    //             //광고 다보면.
    //             BlockManager.instance.OneMoreAdsAfter();

    //             GameManager.instance.curGameState = GameState.SelectBallDirection;
    //             this.gameObject.SetActive(false);
    //             break;
    //         case ShowResult.Skipped:
    //             Debug.Log("The ad was skipped before reaching the end.");
    //             break;
    //         case ShowResult.Failed:
    //             Debug.LogError("The ad failed to be shown.");
    //             break;
    //     }
    // }
}
